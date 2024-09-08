using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag List UI Tab")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBagUI.png")]
    
    public class BagListUITab : MonoBehaviour,
        IPointerClickHandler,
        ISelectHandler,
        ISubmitHandler
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private BagListUI m_BagListUI;
        [SerializeField] private Item m_FilterByParent;
        [SerializeField] private GameObject m_ActiveFilter;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsActive { get; private set; } = false;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            if (this.m_BagListUI == null) return;
            this.m_BagListUI.EventRefreshUI -= this.RefreshUI;
            this.m_BagListUI.EventRefreshUI += this.RefreshUI;

            this.RefreshUI();
        }

        private void OnDisable()
        {
            if (this.m_BagListUI == null) return;
            this.m_BagListUI.EventRefreshUI -= this.RefreshUI;
        }

        // CALLBACKS: -----------------------------------------------------------------------------
        
        public void OnPointerClick(PointerEventData data) => this.Filter();
        public void OnSelect(BaseEventData eventData) => this.Filter();
        public void OnSubmit(BaseEventData data) => this.Filter();

        private void RefreshUI()
        {
            if (this.m_BagListUI == null) return;
            
            if (this.m_ActiveFilter != null)
            {
                Item currentFilter = this.m_BagListUI.FilterByParent;
                
                this.IsActive = this.m_FilterByParent == currentFilter;
                this.m_ActiveFilter.SetActive(this.IsActive);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Filter()
        {
            if (this.m_BagListUI == null) return;
            this.m_BagListUI.FilterByParent = this.m_FilterByParent;
        }
    }
}