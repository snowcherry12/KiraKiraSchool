using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Catalogue
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Dictionary<IdString, Item> m_Map;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Item[] m_Items = Array.Empty<Item>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Item[] List => this.m_Items;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Item Get(IdString itemID)
        {
            this.RequireInitialize();
            return this.m_Map.TryGetValue(itemID, out Item item) ? item : null;
        }

        private void RequireInitialize()
        {
            if (this.m_Map != null) return;
            
            this.m_Map = new Dictionary<IdString, Item>();
            foreach (Item item in this.m_Items) this.m_Map[item.ID] = item;
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------
        
        internal void Set(Item[] items)
        {
            this.m_Items = items;
        }
    }
}