using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public abstract class TItemList<T> where T : IItemListEntry
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private bool m_InheritFromParent = true;
        [SerializeField] protected T[] m_List = Array.Empty<T>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public int ListLength => this.m_List.Length;
        
        public bool InheritFromParent => this.m_InheritFromParent;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        protected TItemList()
        { }
        
        // METHODS: -------------------------------------------------------------------------------

        public T Get(int index) => this.m_List[index];
    }
}