using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public struct TokenBagCooldowns
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private Cooldowns m_Cooldowns;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Cooldowns Cooldowns
        {
            get => this.m_Cooldowns;
            internal set => this.m_Cooldowns = value;
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenBagCooldowns(IBagCooldowns bagCooldown)
        {
            if (bagCooldown == null)
            {
                this.m_Cooldowns = new Cooldowns();
                return;
            }

            Cooldowns cooldownsList = bagCooldown.Cooldowns;
            this.m_Cooldowns = new Cooldowns();
            
            foreach (KeyValuePair<IdString, Cooldown> entry in cooldownsList)
            {
                this.m_Cooldowns[entry.Key] = new Cooldown(entry.Value);
            }
        }
    }
}