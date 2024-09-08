using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public struct TokenBagWealth
    {
        [Serializable]
        public struct Data
        {
            // MEMBERS: ---------------------------------------------------------------------------
            
            [SerializeField] private IdString m_CurrencyID;
            [SerializeField] private int m_Amount;
            
            // PROPERTIES: ------------------------------------------------------------------------

            public IdString CurrencyID => this.m_CurrencyID;
            public int Amount => this.m_Amount;
            
            // CONSTRUCTOR: -----------------------------------------------------------------------

            public Data(IdString currencyID, int amount)
            {
                this.m_CurrencyID = currencyID;
                this.m_Amount = amount;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private Data[] m_Wealth;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Data[] Wealth
        {
            get => this.m_Wealth;
            internal set => this.m_Wealth = value;
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenBagWealth(IBagWealth bagWealth)
        {
            if (bagWealth == null)
            {
                this.m_Wealth = Array.Empty<Data>();
                return;
            }

            List<Data> wealth = new List<Data>();
            foreach (IdString currencyID in bagWealth.List)
            {
                Data data = new Data(
                    currencyID,
                    bagWealth.Get(currencyID)
                );
                
                wealth.Add(data);
            }

            this.m_Wealth = wealth.ToArray();
        }
    }
}