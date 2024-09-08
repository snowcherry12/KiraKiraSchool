using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class CoinSelect
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Currency m_Currency;
        [SerializeField] private int m_CoinIndex = 0;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Currency Currency
        {
            get
            {
                if (this.m_Currency == null) throw new NullReferenceException("Currency is null");
                return this.m_Currency;
            }
        }

        public Coin Coin
        {
            get
            {
                int index = Mathf.Clamp(this.m_CoinIndex, 0, this.Currency.Coins.Length);
                return this.Currency.Coins[index];
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public CoinSelect()
        {
            #if UNITY_EDITOR

            if (this.m_Currency != null) return;

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Currency");
            if (guids.Length == 0) return;

            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            Currency currency = UnityEditor.AssetDatabase.LoadAssetAtPath<Currency>(path);
            
            this.m_Currency = currency;

            #endif
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public int GetCoinAmount(int amount)
        {
            int[] amounts = this.Currency.Coins.Values(amount);
            return amounts[this.m_CoinIndex];
        }
    }
}