using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [DisallowMultipleComponent]
    
    [AddComponentMenu("Game Creator/UI/Quests/Task UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoTaskUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class TaskUI : TTaskUI
    {
        public static event Action<Journal, Quest, int> EventSelect;
        
        public static Quest UI_LastTaskQuestSelected;
        public static int UI_LastTaskTaskSelected;
        
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventSelect = null;
            UI_LastTaskQuestSelected = null;
            UI_LastTaskTaskSelected = 0;
        }
        
        #endif
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void RefreshUI(Journal journal, Quest quest, int taskId)
        {
            this.Refresh(journal, quest, taskId);
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------
        
        public static void SelectTaskUI(Journal journal, Quest quest, int taskId)
        {
            UI_LastTaskQuestSelected = quest;
            UI_LastTaskTaskSelected = taskId;
            
            EventSelect?.Invoke(journal, quest, taskId);
        }
        
        public static void DeselectTaskUI()
        {
            UI_LastTaskQuestSelected = null;
            UI_LastTaskTaskSelected = 0;
            
            EventSelect?.Invoke(null, null, 0);
        }
    }
}