using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [Serializable]
    public class TActiveUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private GameObject m_ActiveIfInactive;
        [SerializeField] private GameObject m_ActiveIfActive;
        
        [SerializeField] private GameObject m_ActiveIfCompleted;
        [SerializeField] private GameObject m_ActiveIfAbandoned;
        [SerializeField] private GameObject m_ActiveIfFailed;
        
        // REFRESH METHODS: -----------------------------------------------------------------------

        protected void Refresh(State state)
        {
            if (this.m_ActiveIfInactive != null) this.m_ActiveIfInactive.SetActive(state == State.Inactive);
            if (this.m_ActiveIfActive != null) this.m_ActiveIfActive.SetActive(state == State.Active);
            
            if (this.m_ActiveIfCompleted != null) this.m_ActiveIfCompleted.SetActive(state == State.Completed);
            if (this.m_ActiveIfAbandoned != null) this.m_ActiveIfAbandoned.SetActive(state == State.Abandoned);
            if (this.m_ActiveIfFailed != null) this.m_ActiveIfFailed.SetActive(state == State.Failed);
        }
    }
}