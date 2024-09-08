using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class TinkerDismantlingUI : TTinkerUI
    {
        [SerializeField] private DismantlingItemUI m_SelectedUI;
        [SerializeField] private RectTransform m_DismantlingContent;
        [SerializeField] private GameObject m_DismantlingItemUIPrefab;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override TTinkerItemUI SelectedUI => this.m_SelectedUI;
        protected override RectTransform Content => this.m_DismantlingContent;
        protected override GameObject Prefab => this.m_DismantlingItemUIPrefab;
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override List<RuntimeItem> GetList(TinkerUI tinkerUI)
        {
            Dictionary<IdString, RuntimeItem> runtimeItems = new Dictionary<IdString, RuntimeItem>();
            List<Cell> cells = tinkerUI.InputBag.Content.CellList;
            
            foreach (Cell cell in cells)
            {
                if (cell == null || cell.Available) continue;
                if (!cell.Item.Crafting.AllowToDismantle) continue;
                if (runtimeItems.ContainsKey(cell.Item.ID)) continue;
                
                runtimeItems.Add(cell.Item.ID, cell.RootRuntimeItem);
            }

            return new List<RuntimeItem>(runtimeItems.Values);
        }
        
        protected override void RefreshItemUI(TinkerUI tinkerUI, Transform child, RuntimeItem runtimeItem)
        {
            DismantlingItemUI dismantlingUI = child.Get<DismantlingItemUI>();
            if (dismantlingUI == null) return;
            
            dismantlingUI.RefreshUI(tinkerUI, runtimeItem);
        }
        
        protected override bool ValidSelection(TinkerUI tinkerUI)
        {
            return tinkerUI.Selection is DismantlingItemUI;
        }
    }
}