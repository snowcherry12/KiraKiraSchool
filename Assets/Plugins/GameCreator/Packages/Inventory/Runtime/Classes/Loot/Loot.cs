using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [System.Serializable]
    public class Loot : TPolymorphicItem<Loot>
    {
        private enum LootType
        {
            Item,
            Currency
        }
        
        private enum AmountMode
        {
            Constant,
            Range
        }

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private int m_Rate = 1;
        [SerializeField] private LootType m_LootType = LootType.Item;
        [SerializeField] private Item m_Item;
        [SerializeField] private Currency m_Currency;

        [SerializeField] private AmountMode m_Amount = AmountMode.Constant;
        [SerializeField] private int m_AmountConstant = 1;
        [SerializeField] private int m_AmountMinimum = 5;
        [SerializeField] private int m_AmountMaximum = 10;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public int Rate => this.m_Rate;
        
        public bool IsItem => this.m_LootType == LootType.Item;
        public bool IsCurrency => this.m_LootType == LootType.Currency;
        
        public Item Item => this.m_Item;
        public Currency Currency => this.m_Currency;

        public int Amount => this.m_Amount switch
        {
            AmountMode.Constant => this.m_AmountConstant,
            AmountMode.Range => Random.Range(this.m_AmountMinimum, this.m_AmountMaximum + 1),
            _ => throw new System.ArgumentOutOfRangeException()
        };

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Loot()
        { }

        public Loot(Item item, int amountConstant, int rate)
        {
            this.m_Item = item;
            this.m_Amount = AmountMode.Constant;
            this.m_AmountConstant = amountConstant;
            
            this.m_Rate = rate;
        }
    }
}