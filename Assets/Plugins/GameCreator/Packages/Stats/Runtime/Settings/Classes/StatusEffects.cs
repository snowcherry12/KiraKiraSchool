using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class StatusEffects
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private StatusEffect[] m_List = Array.Empty<StatusEffect>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public StatusEffect[] List => this.m_List;

        // METHODS: -------------------------------------------------------------------------------

        public StatusEffect Find(int statusEffectHash)
        {
            foreach (StatusEffect statusEffect in this.m_List)
            {
                if (statusEffect == null) continue;
                if (statusEffect.ID.Hash == statusEffectHash) return statusEffect;
            }

            return null;
        }

        internal void SetList(StatusEffect[] statusEffects)
        {
            this.m_List = statusEffects;
        }
    }
}