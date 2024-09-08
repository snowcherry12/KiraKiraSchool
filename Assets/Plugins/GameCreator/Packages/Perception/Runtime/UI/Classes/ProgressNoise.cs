using System;
using UnityEngine;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [Serializable]
    public class ProgressNoise : TProgressSection
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private GameObject m_ActiveIfAudible;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void Refresh(float ratio, bool above)
        {
            base.Refresh(ratio, above);

            bool audible = ratio > float.Epsilon && above;
            if (this.m_ActiveIfAudible != null) this.m_ActiveIfAudible.SetActive(audible);
        }
    }
}