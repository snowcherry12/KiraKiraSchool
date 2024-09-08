using System;
using GameCreator.Runtime.Inventory;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class TokenBag : Token
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private TokenBagShape m_Shape;
        [SerializeField] private TokenBagItems m_Items;
        [SerializeField] private TokenBagWealth m_Wealth;
        [SerializeField] private TokenBagEquipment m_Equipment;
        [SerializeField] private TokenBagCooldowns m_Cooldowns;

        // PROPERTIES: ----------------------------------------------------------------------------

        public TokenBagShape Shape => this.m_Shape;
        public TokenBagItems Items => this.m_Items;
        public TokenBagWealth Wealth => this.m_Wealth;
        public TokenBagEquipment Equipment => this.m_Equipment;
        public TokenBagCooldowns Cooldowns => this.m_Cooldowns;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenBag(Bag target, bool shape, bool items, bool wealth, bool equipment, bool cooldowns) : base()
        {
            bool hasBag = target != null;
            
            this.m_Shape = new TokenBagShape(shape && hasBag ? target.Shape : null);
            this.m_Items = new TokenBagItems(items && hasBag ? target.Content : null);
            this.m_Wealth = new TokenBagWealth(wealth && hasBag ? target.Wealth : null);
            this.m_Equipment = new TokenBagEquipment(equipment && hasBag ? target.Equipment : null);
            this.m_Cooldowns = new TokenBagCooldowns(cooldowns && hasBag ? target.Cooldowns : null);
        }
    }
}