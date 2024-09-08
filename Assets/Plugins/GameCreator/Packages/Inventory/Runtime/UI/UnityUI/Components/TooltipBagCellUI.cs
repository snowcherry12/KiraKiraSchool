using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Tooltip Cell UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoTooltipUI.png")]
    
    public class TooltipBagCellUI : TTooltipUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private RuntimeItemUI m_ItemUI = new RuntimeItemUI();
        [SerializeField] protected CellMerchantUI m_CellMerchant = new CellMerchantUI();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [NonSerialized] private RuntimeItem m_RuntimeItem;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable()
        {
            base.OnEnable();
            BagCellUI.EventHoverEnter -= this.OnHoverEnter;
            BagCellUI.EventHoverEnter += this.OnHoverEnter;
            
            BagCellUI.EventHoverExit -= this.OnHoverExit;
            BagCellUI.EventHoverExit += this.OnHoverExit;
        }
        
        protected override void OnDisable()
        {
            base.OnEnable();
            BagCellUI.EventHoverEnter -= this.OnHoverEnter;
            BagCellUI.EventHoverExit -= this.OnHoverExit;
        }

        private void Update()
        {
            this.m_ItemUI.RefreshCooldown(
                this.m_RuntimeItem?.Bag, 
                this.m_RuntimeItem?.Item
            );
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnHoverEnter(BagCellUI cellUI)
        {
            this.m_RuntimeItem = RuntimeItem.UI_LastItemHovered;
            if (this.m_RuntimeItem == null) return;

            if (!this.CheckBagConditions(this.m_RuntimeItem?.Bag)) return;
            
            this.m_ItemUI.RefreshUI(this.m_RuntimeItem.Bag, this.m_RuntimeItem, true, true);
            this.m_CellMerchant.RefreshUI(cellUI.BagUI, cellUI.Position);
            
            this.SetTooltip(true);
        }

        private void OnHoverExit(BagCellUI cellUI)
        {
            this.SetTooltip(false);
        }
    }
}