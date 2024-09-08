using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag List UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBagUI.png")]
    
    [Serializable]
    public class BagListUI : TBagUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Item m_FilterByParent;
        [SerializeField] private RectTransform m_Content;
        
        [SerializeField] private bool m_HideEquipped;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override Item FilterByParent
        {
            get => this.m_FilterByParent;
            set
            {
                this.m_FilterByParent = value;
                this.RefreshUI();
            }
        }
        
        public override Type ExpectedBagType => typeof(BagList);
        
        protected override RectTransform Content => this.m_Content;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void RefreshUI()
        {
            if (this.Bag == null) return;
            base.RefreshUI();
            
            int cellListCount = this.Bag.Content.CountWithoutStack;
            List<int> cellIndices = new List<int>(cellListCount);
            
            for (int i = 0; i < cellListCount; ++i)
            {
                Vector2Int position = new Vector2Int(0, i);
                Cell cell = this.Bag.Content.GetContent(position);
                
                if (this.m_HideEquipped && this.Bag.Equipment.IsEquipped(cell.RootRuntimeItem))
                {
                    continue;
                }
                
                cellIndices.Add(i);   
            }
            
            int numCells = cellIndices.Count;
            int numChildren = this.m_Content.childCount;

            int numCreate = numCells - numChildren;
            int numDelete = numChildren - numCells;

            for (int i = numCreate - 1; i >= 0; --i) this.CreateCell();
            for (int i = numDelete - 1; i >= 0; --i) this.DeleteCell(numCells + i);
            
            for (int i = 0; i < numCells; ++i)
            {
                Transform child = this.m_Content.GetChild(i);
                BagCellUI cellUI = child.Get<BagCellUI>();
                
                int index = cellIndices[i];
                if (cellUI != null) cellUI.RefreshUI(0, index);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void CreateCell()
        {
            GameObject instance = Instantiate(this.PrefabCell, this.m_Content.transform);
            
            BagCellUI bagCellUI = instance.Get<BagCellUI>();
            if (bagCellUI != null) bagCellUI.OnCreate(this);
        }
        
        private void DeleteCell(int index)
        {
            Transform child = this.m_Content.transform.GetChild(index);
            Destroy(child.gameObject);
        }
    }
}