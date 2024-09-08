using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [DisallowMultipleComponent]
    
    [AddComponentMenu("Game Creator/UI/Quests/Quest UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoQuestUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class QuestUI : TQuestUI
    {
        public static event Action<Journal, Quest> EventSelect;

        public static Quest UI_LastQuestSelected;
        
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventSelect = null;
            UI_LastQuestSelected = null;
        }
        
        #endif
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void RefreshUI(Journal journal, Quest quest)
        {
            this.Refresh(journal, quest);
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------
        
        public static void ToggleTracking(Journal journal, Quest quest)
        {
            switch (journal.IsQuestTracking(quest))
            {
                case true: journal.UntrackQuest(quest); break;
                case false: journal.TrackQuest(quest); break;
            }
        }
        
        public static void SelectQuestUI(Journal journal, Quest quest)
        {
            UI_LastQuestSelected = quest;
            EventSelect?.Invoke(journal, quest);
        }
        
        public static void DeselectQuestUI()
        {
            UI_LastQuestSelected = null;
            EventSelect?.Invoke(null, null);
        }
    }
}