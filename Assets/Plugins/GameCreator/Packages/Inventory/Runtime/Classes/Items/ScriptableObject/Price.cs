using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Price
    {
        private const string ERR_NULL_CURRENCY = "Price has no 'Currency' set";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Currency m_Currency;
        [SerializeField] private int m_Value = 100;

        [SerializeField] private bool m_CanBuyFromMerchant = true;
        [SerializeField] private bool m_CanSellToMerchant = true;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Currency Currency => this.m_Currency != null ? this.m_Currency : null;

        public int ValueRaw => this.m_Value;

        public bool CanBuyFromMerchant => this.m_CanBuyFromMerchant;
        public bool CanSellToMerchant => this.m_CanSellToMerchant;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Price()
        { }

        public Price(int value) : this()
        {
            this.m_Value = value;
        }

        public Price(Currency currency, int value) : this(value)
        {
            this.m_Currency = currency;
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static int GetValue(RuntimeItem runtimeItem)
        {
            int value = runtimeItem.Item.Price.m_Value;
            foreach (KeyValuePair<IdString, RuntimeSocket> entry in runtimeItem.Sockets)
            {
                RuntimeSocket socket = entry.Value;
                if (socket.HasAttachment) value += GetValue(socket.Attachment);
            }

            return value;
        }
    }
}