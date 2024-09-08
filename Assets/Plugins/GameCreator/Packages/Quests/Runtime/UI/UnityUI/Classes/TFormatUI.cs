using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public abstract class TFormatUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected Graphic m_GraphicStatus;
        [SerializeField] private Color m_ColorIsInactive = ColorTheme.GetDarkTheme(ColorTheme.Type.TextLight);
        [SerializeField] private Color m_ColorIsActive = ColorTheme.GetDarkTheme(ColorTheme.Type.TextNormal);
        [SerializeField] private Color m_ColorIsCompleted = ColorTheme.GetDarkTheme(ColorTheme.Type.Green);
        [SerializeField] private Color m_ColorIsAbandoned = ColorTheme.GetDarkTheme(ColorTheme.Type.Yellow);
        [SerializeField] private Color m_ColorIsFailed = ColorTheme.GetDarkTheme(ColorTheme.Type.Red);

        [SerializeField] private Graphic m_GraphicSelected;
        [SerializeField] private Color m_ColorNormal = ColorTheme.GetDarkTheme(ColorTheme.Type.TextLight);
        [SerializeField] private Color m_ColorSelected = ColorTheme.GetDarkTheme(ColorTheme.Type.TextNormal);

        // REFRESH METHODS: -----------------------------------------------------------------------

        protected void Refresh(State state, bool isSelected)
        {
            if (this.m_GraphicStatus != null)
            {
                this.m_GraphicStatus.color = state switch
                {
                    State.Inactive => this.m_ColorIsInactive,
                    State.Active => this.m_ColorIsActive,
                    State.Completed => this.m_ColorIsCompleted,
                    State.Abandoned => this.m_ColorIsAbandoned,
                    State.Failed => this.m_ColorIsFailed,
                    _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
                };
            }

            if (this.m_GraphicSelected != null)
            {
                this.m_GraphicSelected.color = isSelected
                    ? this.m_ColorSelected
                    : this.m_ColorNormal;
            }
        }
    }
}