using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Compass Item UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoCompassUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class CompassItemUI : MonoBehaviour
    {
        private static readonly Gradient GRADIENT = new Gradient
        {
            alphaKeys = new[]
            {
                new GradientAlphaKey(0f, 0.0f),
                new GradientAlphaKey(1f, 0.1f),
                new GradientAlphaKey(1f, 0.9f),
                new GradientAlphaKey(0f, 1.0f),
            }
        };
        
        private enum OpacityMode
        {
            FadeByDistance,
            FadeByDirection,
            Both
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TextReference m_Text = new TextReference();
        [SerializeField] private Image m_Sprite;
        [SerializeField] private Graphic m_Color;
        
        [SerializeField] private OpacityMode m_Mode = OpacityMode.Both;
        [SerializeField] private CanvasGroup m_Opacity;

        [SerializeField] private TextReference m_Distance = new TextReference();
        [SerializeField] private PropertyGetDecimal m_Multiplier = new PropertyGetDecimal(1f);
        [SerializeField] private PropertyGetString m_Unit = new PropertyGetString("m");

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(TSpotPoi spot, float ratio)
        {
            if (spot == null) return;

            this.m_Text.Text = spot.GetName;
            
            if (this.m_Sprite != null) this.m_Sprite.overrideSprite = spot.GetSprite;
            if (this.m_Color != null) this.m_Color.color = spot.GetColor;

            if (this.m_Opacity != null)
            {
                float alpha = this.m_Mode switch
                {
                    OpacityMode.FadeByDistance => spot.Alpha,
                    OpacityMode.FadeByDirection => GRADIENT.Evaluate(ratio).a,
                    OpacityMode.Both => Math.Min(spot.Alpha, GRADIENT.Evaluate(ratio).a),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                this.m_Opacity.alpha = alpha;
            }

            float distance = spot.Hotspot.Distance * (float) this.m_Multiplier.Get(spot.Args);
            this.m_Distance.Text = $"{distance:0}{this.m_Unit.Get(spot.Args)}";
        }
    }
}