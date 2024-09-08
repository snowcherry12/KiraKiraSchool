using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Quest List UI Tab")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoQuestListUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    public class QuestListUITab : MonoBehaviour, IPointerClickHandler, ISubmitHandler
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private QuestListUI m_QuestListUI;

        [SerializeField] private StateFlags m_FilterBy = StateFlags.Active;
        [SerializeField] private GameObject m_ActiveFilter;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            if (this.m_QuestListUI == null) return;
            this.m_QuestListUI.EventRefreshUI -= this.RefreshUI;
            this.m_QuestListUI.EventRefreshUI += this.RefreshUI;

            this.RefreshUI();
        }

        private void OnDisable()
        {
            if (this.m_QuestListUI == null) return;
            this.m_QuestListUI.EventRefreshUI -= this.RefreshUI;
        }

        // CALLBACKS: -----------------------------------------------------------------------------
        
        public void OnPointerClick(PointerEventData data) => this.Filter();
        public void OnSubmit(BaseEventData data) => this.Filter();

        private void RefreshUI()
        {
            if (this.m_QuestListUI == null) return;
            if (this.m_ActiveFilter == null) return;
            
            StateFlags currentFilter = this.m_QuestListUI.Show;
            this.m_ActiveFilter.SetActive(this.m_FilterBy == currentFilter);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Filter()
        {
            if (this.m_QuestListUI == null) return;
            if (this.m_QuestListUI.Show == this.m_FilterBy) return;
            
            TaskUI.DeselectTaskUI(); 
            QuestUI.DeselectQuestUI();
            
            this.m_QuestListUI.Show = this.m_FilterBy;
        }
    }
}
