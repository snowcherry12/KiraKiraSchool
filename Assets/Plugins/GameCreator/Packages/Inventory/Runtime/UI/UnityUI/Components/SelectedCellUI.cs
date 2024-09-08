using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Selected Cell UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBagUI.png")]
    
    public class SelectedCellUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private RuntimeItemUI m_ItemUI = new RuntimeItemUI();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private BagCellUI m_CellUI;
        [NonSerialized] private RuntimeItem m_RuntimeItem;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            BagCellUI.EventSelect -= this.OnSelect;
            BagCellUI.EventSelect += this.OnSelect;
            
            this.m_ItemUI.RefreshUI(null, null, false, true);
        }
        
        private void OnDisable()
        {
            BagCellUI.EventHoverEnter -= this.OnSelect;
            BagCellUI.EventHoverExit -= this.OnDeselect;
        }

        private void Update()
        {
            this.m_ItemUI.RefreshCooldown(
                this.m_RuntimeItem?.Bag, 
                this.m_RuntimeItem?.Item
            );
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnSelect(BagCellUI cellUI)
        {
            if (this.m_CellUI != null && this.m_CellUI.BagUI != null)
            {
                this.m_CellUI.BagUI.EventRefreshUI -= this.RefreshUI;
            }

            this.m_RuntimeItem = cellUI.Cell?.RootRuntimeItem;
            this.m_CellUI = cellUI;
            this.m_CellUI.BagUI.EventRefreshUI += this.RefreshUI;

            this.RefreshUI();
        }

        private void OnDeselect(BagCellUI cellUI)
        {
            RuntimeItem runtimeItem = RuntimeItem.UI_LastItemSelected;
            this.m_ItemUI.RefreshUI(runtimeItem?.Bag, runtimeItem, false, true);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI()
        {
            if (this.m_CellUI == null || 
                this.m_CellUI.BagUI == null || 
                !this.m_CellUI.BagUI.Bag.Content.Contains(this.m_RuntimeItem))
            {
                this.m_RuntimeItem = null;
            }
            
            this.m_ItemUI.RefreshUI(
                this.m_RuntimeItem?.Bag, 
                this.m_RuntimeItem,
                this.m_CellUI != null && this.m_CellUI.BagUI && this.m_RuntimeItem != null,
                true
            );
        }
        
        public void Use()
        {
            this.m_CellUI.Use();
        }

        public void Equip()
        {
            this.m_CellUI.Equip();
        }

        public void Unequip()
        {
            this.m_CellUI.Unequip();
        }
        
        public void Drop()
        {
            this.m_CellUI.Drop();
        }

        public void Dismantle(float chance)
        {
            this.m_CellUI.Dismantle(chance);
        }

        public void Split()
        {
            this.m_CellUI.Split();
        }
    }
}