using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Socket : TPolymorphicItem<Socket>, IItemListEntry
    {
        [SerializeField] private Item m_Base;
        [SerializeField] private IdString m_SocketID;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Socket Clone => new Socket
        {
            m_Base = this.m_Base,
            m_SocketID = this.m_SocketID
        };
        
        public Item Base => this.m_Base;
        public IdString ID => this.m_SocketID;
    }
}