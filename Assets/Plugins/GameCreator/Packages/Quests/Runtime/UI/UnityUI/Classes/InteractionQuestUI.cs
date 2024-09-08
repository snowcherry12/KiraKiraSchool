using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class InteractionQuestUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Button m_ButtonTrack;
        [SerializeField] private Selectable m_SelectQuest;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private TQuestUI m_QuestUI;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Setup(TQuestUI questUI)
        {
            this.m_QuestUI = questUI;

            if (this.m_ButtonTrack != null)
            {
                this.m_ButtonTrack.onClick.RemoveListener(this.OnTrack);
                this.m_ButtonTrack.onClick.AddListener(this.OnTrack);
            }

            if (this.m_SelectQuest != null)
            {
                SelectableHelper.Register(this.m_SelectQuest, this.OnSelect, null);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void OnTrack()
        {
            this.m_QuestUI.ToggleTracking();
        }
        
        private void OnSelect()
        {
            this.m_QuestUI.Select();
        }
    }
}