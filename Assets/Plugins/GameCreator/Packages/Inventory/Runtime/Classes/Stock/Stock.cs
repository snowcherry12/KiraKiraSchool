using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Stock
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private StockData[] m_Stock = Array.Empty<StockData>();
        [SerializeReference] private WealthData[] m_Wealth = Array.Empty<WealthData>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public int StockLength => this.m_Stock.Length;
        public int WealthLength => this.m_Wealth.Length;

        // GETTERS: -------------------------------------------------------------------------------

        public Item GetStockItem(int index) => this.m_Stock[index].Item;
        public int GetStockAmount(int index) => this.m_Stock[index].Amount;
        
        public Currency GetWealthCurrency(int index) => this.m_Wealth[index].Currency;
        public int GetWealthAmount(int index) => this.m_Wealth[index].Amount;
    }
}