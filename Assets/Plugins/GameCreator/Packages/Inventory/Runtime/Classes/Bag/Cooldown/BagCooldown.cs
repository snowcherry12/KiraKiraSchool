using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagCooldowns : IBagCooldowns
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected Cooldowns m_Cooldowns = new Cooldowns();

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] protected Bag Bag { get; private set; }

        public virtual Cooldowns Cooldowns => this.m_Cooldowns;

        // INITIALIZERS METHODS: ------------------------------------------------------------------

        public virtual void OnAwake(Bag bag)
        {
            this.Bag = bag;
            this.m_Cooldowns = new Cooldowns();
        }

        public void OnLoad(TokenBagCooldowns tokenBagCooldowns)
        {
            this.m_Cooldowns.Clear();
            foreach (KeyValuePair<IdString, Cooldown> entry in tokenBagCooldowns.Cooldowns)
            {
                this.m_Cooldowns[entry.Key] = new Cooldown(entry.Value);
            }
        }
        
        // GETTERS: -------------------------------------------------------------------------------

        public virtual Cooldown GetCooldown(Item item)
        {
            return item != null && this.m_Cooldowns.TryGetValue(item.ID, out Cooldown cooldown) 
                ? cooldown 
                : null;
        }

        // SETTERS: -------------------------------------------------------------------------------
        
        public virtual void SetCooldown(Item item, Args args)
        {
            if (item == null) return;
            
            if (this.m_Cooldowns.TryGetValue(item.ID, out Cooldown cooldown))
            {
                cooldown.Set(item, args);
            }
            else
            {
                cooldown = new Cooldown(item, args);
                this.m_Cooldowns.Add(item.ID, cooldown);
            }
        }

        public virtual void ResetCooldown(Item item)
        {
            if (item == null) return;
            if (!this.m_Cooldowns.TryGetValue(item.ID, out Cooldown cooldown)) return;

            cooldown.Reset();
        }

        public virtual void ClearCooldowns()
        {
            this.m_Cooldowns.Clear();
        }
    }
}