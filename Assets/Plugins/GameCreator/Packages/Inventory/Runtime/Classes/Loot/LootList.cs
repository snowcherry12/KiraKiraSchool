using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class LootList
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Loot[] m_List = new Loot[0];
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public Loot[] List => this.m_List;
    }
}