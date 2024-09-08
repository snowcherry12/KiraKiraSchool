using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Sockets : TItemList<Socket>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_PrefabSocket;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject PrefabSocket => this.m_PrefabSocket;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Sockets() : base()
        { }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static Dictionary<IdString, Socket> FlattenHierarchy(Item item)
        {
            Dictionary<IdString, Socket> map = new Dictionary<IdString, Socket>();
            if (item == null) return map;
            
            foreach (Socket listItem in item.Sockets.m_List) map[listItem.ID] = listItem.Clone;

            if (item.Parent != null && item.Sockets.InheritFromParent)
            {
                Dictionary<IdString, Socket> parentList = FlattenHierarchy(item.Parent);
                foreach (KeyValuePair<IdString, Socket> entry in parentList)
                {
                    if (!map.ContainsKey(entry.Key))
                    {
                        map[entry.Key] = entry.Value;
                    }
                }
            }
        
            return map;
        }
    }
}