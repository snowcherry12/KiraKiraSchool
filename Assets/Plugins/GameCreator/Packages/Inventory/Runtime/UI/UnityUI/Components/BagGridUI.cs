using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag Grid UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBagUI.png")]
    
    [Serializable]
    public class BagGridUI : TBagUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GridLayoutGroup m_Content;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override Type ExpectedBagType => typeof(BagGrid);
        
        protected override RectTransform Content => this.m_Content.Get<RectTransform>();

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void RefreshUI()
        {
            if (this.Bag == null) return;
            base.RefreshUI();
            
            this.m_Content.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            this.m_Content.constraintCount = this.Bag.Shape.MaxWidth;
            
            int width = this.Bag.Shape.MaxWidth;
            int height = this.Bag.Shape.MaxHeight;
            int numCells = width * height;
            
            int childCount = this.m_Content.transform.childCount;
            for (int i = childCount; i < numCells; ++i) this.CreateCell();
            
            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    int index = j * width + i;
                    Transform child = this.m_Content.transform.GetChild(index);
            
                    BagCellUI cellUI = child.Get<BagCellUI>();
                    if (cellUI != null) cellUI.RefreshUI(i, j);
                }
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void CreateCell()
        {
            GameObject instance = Instantiate(this.PrefabCell, this.m_Content.transform);
            
            BagCellUI bagCellUI = instance.Get<BagCellUI>();
            if (bagCellUI != null) bagCellUI.OnCreate(this);
        }
    }
}