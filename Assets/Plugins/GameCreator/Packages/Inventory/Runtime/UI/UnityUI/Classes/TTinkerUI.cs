using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [System.Serializable]
    public abstract class TTinkerUI
    {
        [SerializeField] private Item m_FilterByParent;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected abstract TTinkerItemUI SelectedUI { get; }
        protected abstract RectTransform Content { get; }
        protected abstract GameObject Prefab { get; }

        public Item FilterByParent
        {
            get => this.m_FilterByParent;
            set => this.m_FilterByParent = value;
        }

        // ABSTRACT METHODS: ----------------------------------------------------------------------
        
        protected abstract List<RuntimeItem> GetList(TinkerUI tinkerUI); 

        protected abstract void RefreshItemUI(TinkerUI tinkerUI, Transform child, RuntimeItem runtimeItem);

        protected abstract bool ValidSelection(TinkerUI tinkerUI);

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void RefreshUI(TinkerUI tinkerUI)
        {
            if (this.Content == null || this.Prefab == null) return;
            List<RuntimeItem> runtimeItems = this.GetList(tinkerUI);

            if (this.m_FilterByParent != null)
            {
                for (int i = runtimeItems.Count - 1; i >= 0; --i)
                {
                    if (runtimeItems[i].InheritsFrom(this.m_FilterByParent.ID)) continue;
                    runtimeItems.RemoveAt(i);
                }
            }

            int numItems = runtimeItems.Count;
            int numChildren = this.Content.childCount;

            int numCreate = numItems - numChildren;
            int numDelete = numChildren - numItems;

            for (int i = numCreate - 1; i >= 0; --i) this.CreateItem();
            for (int i = numDelete - 1; i >= 0; --i) this.DeleteItem(numItems + i);
            
            for (int i = 0; i < numItems; ++i)
            {
                Transform child = this.Content.GetChild(i);
                this.RefreshItemUI(tinkerUI, child, runtimeItems[i]);
            }
            
            if (this.SelectedUI != null)
            {
                this.SelectedUI.gameObject.SetActive(tinkerUI.Selection != null);
                if (tinkerUI.Selection != null && this.ValidSelection(tinkerUI))
                {
                    this.SelectedUI.RefreshUI(tinkerUI, tinkerUI.Selection.RuntimeItem);
                }
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void CreateItem()
        {
            Object.Instantiate(this.Prefab, this.Content);
        }
        
        private void DeleteItem(int index)
        {
            Transform child = this.Content.GetChild(index);
            Object.Destroy(child.gameObject);
        }
    }
}