using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    public static class TargetUtils
    {
        private static Processor LastProcessor { get; set; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventRefresh;
        
        // PUBLIC PROPERTIES: ---------------------------------------------------------------------
        
        public static Processor Processor => EditorApplication.isPlaying ? LastProcessor : null;
        
        public static RuntimeData Get
        {
            get
            {
                if (!EditorApplication.isPlaying) return null;
                return LastProcessor != null ? LastProcessor.RuntimeData : null;
            }
        }

        // INITIALIZERS: --------------------------------------------------------------------------

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            UnityEditor.Selection.selectionChanged += OnChangeSelection;
            EditorApplication.playModeStateChanged += OnChangePlayMode;
        }
        
        // CALLBACKS: -----------------------------------------------------------------------------
        
        private static void OnChangeSelection()
        {
            RefreshTarget();
        }
        
        private static void OnChangePlayMode(PlayModeStateChange playMode)
        {
            RefreshTarget();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void RefreshTarget()
        {
            GameObject currentSelection = UnityEditor.Selection.activeObject as GameObject;
            if (currentSelection == null) return;
            
            Processor currentProcessor = currentSelection.GetComponent<Processor>();
            if (currentProcessor == null) return;
            
            if (LastProcessor != null)
            {
                LastProcessor.EventAfterIteration -= ScheduleRefresh;
            }
            
            LastProcessor = currentProcessor;
            
            LastProcessor.EventAfterIteration -= ScheduleRefresh;
            LastProcessor.EventAfterIteration += ScheduleRefresh;
            
            ScheduleRefresh();
        }
        
        private static void ScheduleRefresh()
        {
            EditorApplication.delayCall -= ExecuteRefresh;
            EditorApplication.delayCall += ExecuteRefresh;
        }

        private static void ExecuteRefresh()
        {
            EventRefresh?.Invoke();
        }
    }
}