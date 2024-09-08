using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Perception/Indicator Awareness Item UI")]
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoAwarenessUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class IndicatorAwarenessItemUI : MonoBehaviour
    {
        private const float SMALL_EPSILON = 0.001f;
        private const float BIG_EPSILON = 0.010f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Graphic m_Color;
        [SerializeField] private CanvasGroup m_Opacity;

        [SerializeField] private GameObject m_ActiveIfNotZero;
        [SerializeField] private GameObject m_ActiveIfSuspicious;
        [SerializeField] private GameObject m_ActiveIfAlert;
        [SerializeField] private GameObject m_ActiveIfAware;
        
        [SerializeField] private GameObject m_ActiveIfOnscreen;
        [SerializeField] private GameObject m_ActiveIfOffscreen;
        
        [SerializeField] private RectTransform m_RotateTo;

        [SerializeField] private ProgressAwareness m_Awareness = new ProgressAwareness();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_PreviousAwareness = 0;

        // PRIVATE METHODS: -----------------------------------------------------------------------

        public void Refresh(float awareness, Color color, bool onScreen, float rotation)
        {
            if (this.m_Color != null) this.m_Color.color = color;
            if (this.m_Opacity != null) this.m_Opacity.alpha = awareness;

            if (this.m_RotateTo != null)
            {
                Quaternion quaternion = Quaternion.Euler(
                    this.m_RotateTo.localRotation.eulerAngles.x,
                    this.m_RotateTo.localRotation.eulerAngles.y,
                    rotation
                );
                
                this.m_RotateTo.localRotation = quaternion;
            }

            if (this.m_ActiveIfNotZero != null)
            {
                this.m_ActiveIfNotZero.SetActive(this.m_ActiveIfNotZero.activeSelf
                    ? awareness > SMALL_EPSILON
                    : awareness > BIG_EPSILON
                );
            }

            AwareStage stage = Tracker.GetStage(awareness);
            
            if (this.m_ActiveIfSuspicious != null) this.m_ActiveIfSuspicious.SetActive(stage == AwareStage.Suspicious);
            if (this.m_ActiveIfAlert != null) this.m_ActiveIfAlert.SetActive(stage == AwareStage.Alert);
            if (this.m_ActiveIfAware != null) this.m_ActiveIfAware.SetActive(stage == AwareStage.Aware);
            
            if (this.m_ActiveIfOnscreen != null) this.m_ActiveIfOnscreen.SetActive(onScreen);
            if (this.m_ActiveIfOffscreen != null) this.m_ActiveIfOffscreen.SetActive(!onScreen);
            
            this.m_Awareness.Refresh(awareness, false);

            this.m_PreviousAwareness = awareness;
        }
    }
}