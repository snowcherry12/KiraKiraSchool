using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class StockData : TPolymorphicItem<StockData>
    {
        [SerializeField] private Item m_Item;
        [SerializeField] private int m_Amount;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Item Item => this.m_Item;
        public int Amount => this.m_Amount;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StockData()
        {
            this.m_Amount = 1;
        }
    }
}