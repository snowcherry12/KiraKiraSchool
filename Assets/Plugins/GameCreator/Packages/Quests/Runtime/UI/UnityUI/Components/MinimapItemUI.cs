using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Minimap Item UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoMinimapUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class MinimapItemUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TextReference m_Text;
        [SerializeField] private Image m_Sprite;
        [SerializeField] private Graphic m_Color;
        [SerializeField] private CanvasGroup m_Opacity;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(TSpotPoi spot)
        {
            if (spot == null) return;

            this.m_Text.Text = spot.GetName;
            
            if (this.m_Sprite != null) this.m_Sprite.overrideSprite = spot.GetSprite;
            if (this.m_Color != null) this.m_Color.color = spot.GetColor;
            if (this.m_Opacity != null) this.m_Opacity.alpha = spot.Alpha;
        }
    }
}