using System.Collections;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [DisallowMultipleComponent]
    
    [AddComponentMenu("Game Creator/UI/Quests/Selected Task UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoTaskUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    public class SelectedTaskUI : TTaskUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_ActiveIfSelected;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private IEnumerator Start()
        {
            yield return null;
            this.OnSelectTask(this.Journal, this.Quest, this.TaskId);
        }

        private void OnEnable()
        {
            TaskUI.EventSelect -= this.OnSelectTask;
            TaskUI.EventSelect += this.OnSelectTask;
        }

        private void OnDisable()
        {
            TaskUI.EventSelect -= this.OnSelectTask;
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------
        
        private void OnSelectTask(Journal journal, Quest quest, int taskId)
        {
            if (this.m_ActiveIfSelected != null)
            {
                bool isSelected = quest != null && taskId != TasksTree.NODE_INVALID;
                this.m_ActiveIfSelected.SetActive(isSelected);
            }
            
            if (quest == null) return;
            this.Refresh(journal, quest, taskId);
        }
    }
}