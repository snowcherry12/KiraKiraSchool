using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [Serializable]
    public abstract class TProgressSection
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Image m_Fill;
        [SerializeField] private Graphic m_Alpha;
        [SerializeField] private RectTransform m_Scale;

        [SerializeField] private RectTransform m_Right;
        [SerializeField] private RectTransform m_Left;
        [SerializeField] private RectTransform m_Top;
        [SerializeField] private RectTransform m_Bottom;

        [SerializeField] private TextReference m_Percent;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void Refresh(float ratio, bool above)
        {
            if (this.m_Fill != null) this.m_Fill.fillAmount = ratio;
            
            if (this.m_Alpha != null) this.m_Alpha.color = ColorUtils.SetAlpha(
                this.m_Alpha.color,
                ratio
            );
            
            if (this.m_Scale != null) this.m_Scale.localScale = Vector3.one * ratio;

            if (this.m_Right != null) this.m_Right.anchorMax = new Vector2(ratio, 1);
            if (this.m_Left != null) this.m_Left.anchorMin = new Vector2(ratio, 0);
            
            if (this.m_Top != null) this.m_Right.anchorMax = new Vector2(1, ratio);
            if (this.m_Bottom != null) this.m_Left.anchorMin = new Vector2(0, ratio);

            if (this.m_Percent != null) this.m_Percent.Text = (ratio * 100f).ToString("N0");
        }
    }
}