using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class FormatQuestUI : TFormatUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Graphic m_GraphicTracking;
        [SerializeField] private Color m_ColorIsTracking = ColorTheme.GetDarkTheme(ColorTheme.Type.Blue);
        [SerializeField] private Color m_ColorIsNotTracking = ColorTheme.GetDarkTheme(ColorTheme.Type.TextNormal);

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(Journal journal, Quest quest)
        {
            State state = journal.GetQuestState(quest);
            bool isSelected = QuestUI.UI_LastQuestSelected == quest;

            this.Refresh(state, isSelected);

            if (this.m_GraphicTracking != null)
            {
                this.m_GraphicTracking.color = journal.IsQuestTracking(quest)
                    ? this.m_ColorIsTracking
                    : this.m_ColorIsNotTracking;
            }
        }
    }
}