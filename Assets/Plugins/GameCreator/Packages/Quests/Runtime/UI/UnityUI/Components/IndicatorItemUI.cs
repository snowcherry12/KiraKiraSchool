using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Indicator Item UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoIndicatorUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class IndicatorItemUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TextReference m_Text;
        [SerializeField] private Image m_Sprite;
        [SerializeField] private Graphic m_Color;
        [SerializeField] private CanvasGroup m_Opacity;

        [SerializeField] private GameObject m_ActiveIfOnscreen;
        [SerializeField] private GameObject m_ActiveIfOffscreen;
        
        [SerializeField] private RectTransform m_RotateTo;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(TSpotPoi spot, bool onScreen, float rotation)
        {
            if (spot == null) return;

            this.m_Text.Text = spot.GetName;
            
            if (this.m_Sprite != null) this.m_Sprite.overrideSprite = spot.GetSprite;
            if (this.m_Color != null) this.m_Color.color = spot.GetColor;
            if (this.m_Opacity != null) this.m_Opacity.alpha = spot.Alpha;

            if (this.m_RotateTo != null)
            {
                Quaternion quaternion = Quaternion.Euler(
                    this.m_RotateTo.localRotation.eulerAngles.x,
                    this.m_RotateTo.localRotation.eulerAngles.y,
                    rotation
                );
                
                this.m_RotateTo.localRotation = quaternion;
            }
            
            if (this.m_ActiveIfOnscreen != null) this.m_ActiveIfOnscreen.SetActive(onScreen);
            if (this.m_ActiveIfOffscreen != null) this.m_ActiveIfOffscreen.SetActive(!onScreen);
        }
    }
}