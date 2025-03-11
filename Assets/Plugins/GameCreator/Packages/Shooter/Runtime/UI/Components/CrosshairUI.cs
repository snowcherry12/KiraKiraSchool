using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Shooter
{
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoCrosshairUI.png")]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [AddComponentMenu("Game Creator/UI/Shooter/Crosshair UI")]
    [Serializable]
    public class CrosshairUI : MonoBehaviour
    {
        private enum Direction
        {
            None,
            AwayFromPlayer,
            AwayFromScreenCenter
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private RectTransform m_Reticle;
        [SerializeField] private Direction m_Direction = Direction.None;

        [SerializeField] private RectTransform m_AccuracyPosition;
        [SerializeField] private Vector2 m_PositionX = new Vector2(0, 100f);
        [SerializeField] private Vector2 m_PositionY = new Vector2(0, 100f);
        
        [SerializeField] private RectTransform m_AccuracySize;
        [SerializeField] private Vector2 m_Width = new Vector2(0, 100f);
        [SerializeField] private Vector2 m_Height = new Vector2(0, 100f);
        
        [SerializeField] private RectTransform m_AccuracyRotation;
        [SerializeField] private Vector2 m_Rotation =  new Vector2(0, 180f);
        
        [SerializeField] private RectTransform m_AccuracyScale;
        [SerializeField] private Vector2 m_ScaleX = new Vector2(1, 2f);
        [SerializeField] private Vector2 m_ScaleY = new Vector2(1, 2f);
        
        [SerializeField] private Image m_AccuracyFill;
        [SerializeField] private Vector2 m_Fill = new Vector2(0, 1f);

        [SerializeField] private Graphic m_AccuracyAlpha;
        [SerializeField] private Vector2 m_Alpha = new Vector2(1f, 0.5f);

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Canvas m_Canvas;

        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void OnEnable()
        {
            this.m_Canvas = this.GetComponentInParent<Canvas>();
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        public void Refresh(Vector3 point, float accuracy)
        {
            if (this.m_Reticle != null)
            {
                this.RefreshReticle(point);
            }

            if (this.m_AccuracyPosition != null)
            {
                this.m_AccuracyPosition.anchoredPosition = new Vector2(
                    Mathf.Lerp(this.m_PositionX.x, this.m_PositionX.y, accuracy),
                    Mathf.Lerp(this.m_PositionY.x, this.m_PositionY.y, accuracy)
                );
            }
            
            if (this.m_AccuracySize != null)
            {
                this.m_AccuracySize.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal, 
                    Mathf.Lerp(this.m_Width.x, this.m_Width.y, accuracy)
                );
                
                this.m_AccuracySize.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical, 
                    Mathf.Lerp(this.m_Height.x, this.m_Height.y, accuracy)
                );
            }
            
            if (this.m_AccuracyRotation != null)
            {
                this.m_AccuracyRotation.localRotation = Quaternion.Euler(
                    this.m_AccuracyRotation.localRotation.eulerAngles.x,
                    this.m_AccuracyRotation.localRotation.eulerAngles.y,
                    Mathf.Lerp(this.m_Rotation.x, this.m_Rotation.y, accuracy)
                );
            }
            
            if (this.m_AccuracyScale != null)
            {
                this.m_AccuracyScale.localScale = new Vector2(
                    Mathf.Lerp(this.m_ScaleX.x, this.m_ScaleX.y, accuracy),
                    Mathf.Lerp(this.m_ScaleY.x, this.m_ScaleY.y, accuracy)
                );
            }
            
            if (this.m_AccuracyFill != null)
            {
                this.m_AccuracyFill.fillAmount = Mathf.Lerp(
                    this.m_Fill.x,
                    this.m_Fill.y,
                    accuracy
                );
            }
            
            if (this.m_AccuracyAlpha != null)
            {
                this.m_AccuracyAlpha.color = new Color(
                    this.m_AccuracyAlpha.color.r,
                    this.m_AccuracyAlpha.color.g,
                    this.m_AccuracyAlpha.color.b,
                    Mathf.Lerp(this.m_Alpha.x, this.m_Alpha.y, accuracy)
                );
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshReticle(Vector3 point)
        {
            RectTransform reticleParent = (RectTransform) this.m_Reticle.parent;
            Camera mainCamera = ShortcutMainCamera.Instance.Get<Camera>();

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
                mainCamera,
                point
            );

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                reticleParent,
                screenPoint,
                this.m_Canvas.worldCamera,
                out Vector2 reticlePosition
            );

            this.m_Reticle.pivot = new Vector2(0.5f, 0.5f);
            this.m_Reticle.anchorMin = new Vector2(0.5f, 0.5f);
            this.m_Reticle.anchorMax = new Vector2(0.5f, 0.5f);
            this.m_Reticle.anchoredPosition = reticlePosition;

            switch (this.m_Direction)
            {
                case Direction.None:
                    this.m_Reticle.localRotation = Quaternion.Euler(
                        this.m_Reticle.localRotation.eulerAngles.x,
                        this.m_Reticle.localRotation.eulerAngles.y,
                        0f
                    );
                    break;
                
                case Direction.AwayFromPlayer:
                    Vector2 fromPlayerDirection = Vector2.zero;
                    if (ShortcutPlayer.Transform != null)
                    {
                        Vector3 screenPlayer = mainCamera.WorldToScreenPoint(
                            ShortcutPlayer.Transform.position
                        );
                        fromPlayerDirection = screenPoint - screenPlayer.XY();
                    }
                    this.m_Reticle.localRotation = Quaternion.Euler(
                        this.m_Reticle.localRotation.eulerAngles.x,
                        this.m_Reticle.localRotation.eulerAngles.y,
                        fromPlayerDirection != Vector2.zero
                            ? Vector2.SignedAngle(Vector2.up, fromPlayerDirection)
                            : 0f
                    );
                    break;
                
                case Direction.AwayFromScreenCenter:
                    Vector2 screenCenter = new Vector2(
                        mainCamera.pixelWidth * 0.5f,
                        mainCamera.pixelHeight * 0.5f
                    );
                    Vector2 fromScreenCenterDirection = screenPoint - screenCenter;
                    this.m_Reticle.localRotation = Quaternion.Euler(
                        this.m_Reticle.localRotation.eulerAngles.x,
                        this.m_Reticle.localRotation.eulerAngles.y,
                        fromScreenCenterDirection != Vector2.zero
                            ? Vector2.SignedAngle(Vector2.up, fromScreenCenterDirection)
                            : 0f
                    );
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}