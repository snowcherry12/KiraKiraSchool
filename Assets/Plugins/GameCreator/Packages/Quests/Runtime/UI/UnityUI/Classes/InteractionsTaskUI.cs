using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class InteractionsTaskUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Selectable m_SelectTask;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private TTaskUI m_TaskUI;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Setup(TTaskUI taskUI)
        {
            this.m_TaskUI = taskUI;

            if (this.m_SelectTask != null)
            {
                SelectableHelper.Register(this.m_SelectTask, this.OnSelect, null);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnSelect()
        {
            this.m_TaskUI.Select();
        }
    }
}