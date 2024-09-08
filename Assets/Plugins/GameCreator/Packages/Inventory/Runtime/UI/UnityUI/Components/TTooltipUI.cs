using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [RequireComponent(typeof(RectTransform))]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoTooltipUI.png")]
    
    public abstract class TTooltipUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected RectTransform m_Tooltip;
        [SerializeField] private Vector2 m_TooltipOffset = Vector2.zero;
        [SerializeField] private bool m_KeepInParent = true;
        
        [SerializeField]
        private InputPropertyValueVector2 m_InputMouse = InputValueVector2MousePosition.Create();
        
        [SerializeField] private AnyOrBag m_FromBag = new AnyOrBag();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Canvas m_Canvas;
        [NonSerialized] private RectTransform m_RectTransform;
        
        [NonSerialized] private readonly Vector3[] m_ParentCorners = new Vector3[4];

        // INITIALIZERS: --------------------------------------------------------------------------

        protected void Awake()
        {
            this.m_Canvas = this.GetComponentInParent<Canvas>();
            this.m_RectTransform = this.GetComponent<RectTransform>();
            
            this.m_InputMouse.OnStartup();
            this.SetTooltip(false);
        }

        protected virtual void OnEnable()
        {
            this.SetTooltip(false);
        }

        protected virtual void OnDisable()
        {
            this.SetTooltip(false);
        }

        protected void OnDestroy()
        {
            this.m_InputMouse.OnDispose();
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        protected virtual void LateUpdate()
        {
            this.m_InputMouse.OnUpdate();
            
            bool valid = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.m_RectTransform,
                this.m_InputMouse.Read(),
                this.m_Canvas.worldCamera,
                out Vector2 position
            );
            
            if (!valid) return;
            
            position += this.m_TooltipOffset;
            this.m_Tooltip.localPosition = position;

            if (this.m_KeepInParent)
            {
                this.m_Tooltip.anchorMin = Vector2.zero;
                this.m_Tooltip.anchorMax = Vector2.zero;
                
                this.m_RectTransform.GetLocalCorners(this.m_ParentCorners);

                float offsetX = 0f;
                float offsetY = 0f;

                Bounds tooltipBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(
                    this.m_RectTransform,
                    this.m_Tooltip
                );

                Vector3 parentCornerTopLeft = this.m_ParentCorners[1];
                Vector3 parentCornerBottomRight = this.m_ParentCorners[3];
                
                if (tooltipBounds.min.x < parentCornerTopLeft.x)
                {
                    offsetX = parentCornerTopLeft.x - tooltipBounds.min.x;
                }
                
                if (tooltipBounds.min.y < parentCornerBottomRight.y)
                {
                    offsetY = parentCornerBottomRight.y - tooltipBounds.min.y;
                }

                if (tooltipBounds.max.x > parentCornerBottomRight.x)
                {
                    offsetX = parentCornerBottomRight.x - tooltipBounds.max.x;
                }

                if (tooltipBounds.max.y > parentCornerTopLeft.y)
                {
                    offsetY = parentCornerTopLeft.y - tooltipBounds.max.y;
                }

                this.m_Tooltip.anchoredPosition += new Vector2(offsetX, offsetY);
            }
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void SetTooltip(bool active)
        {
            if (this.m_Tooltip == null) return;
            this.m_Tooltip.gameObject.SetActive(active);
        }

        protected bool CheckBagConditions(Bag bag)
        {
            return bag != null && this.m_FromBag.Match(bag, bag.Args);
        }
    }
}