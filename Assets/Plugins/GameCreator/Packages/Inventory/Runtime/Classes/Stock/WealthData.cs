using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class WealthData : TPolymorphicItem<StockData>
    {
        [SerializeField] private Currency m_Currency;
        [SerializeField] private int m_Amount;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Currency Currency => this.m_Currency;
        public int Amount => this.m_Amount;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public WealthData()
        {
            this.m_Amount = 1;
        }
    }
}