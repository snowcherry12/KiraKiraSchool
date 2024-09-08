using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{ 
    [Title("Bag Type")]
    
    [Serializable]
    public abstract class TBag : IBag
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private BagEquipment m_Equipment = new BagEquipment();
        [SerializeField] private BagCooldowns m_Cooldown = new BagCooldowns();
        [SerializeField] private BagWealth m_Wealth = new BagWealth();

        // Requires implementation of: [SerializeField] m_Shape   : IBagShape
        // Requires implementation of: [SerializeField] m_Content : IBagContent
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected Bag Bag { get; private set; }

        public abstract IBagShape Shape { get; }
        public abstract IBagContent Content { get; }

        public IBagEquipment Equipment => this.m_Equipment;
        public IBagCooldowns Cooldowns => this.m_Cooldown;

        public BagWealth Wealth => this.m_Wealth;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public void OnAwake(Bag bag)
        {
            this.Bag = bag;

            this.Shape.OnAwake(bag);
            this.Content.OnAwake(bag);
            this.Equipment.OnAwake(bag);
            this.Cooldowns.OnAwake(bag);
        }
    }
}