using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Shape
    {
        [SerializeField] private int m_Width = 1;
        [SerializeField] private int m_Height = 1;
        
        [SerializeField] private int m_Weight = 1;
        [SerializeField] private int m_MaxStack = 1;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public int Width => this.m_Width;
        public int Height => this.m_Height;

        public int Weight => this.m_Weight;
        public int MaxStack => this.m_MaxStack;

        // SERIALIZATION CALLBACK: ----------------------------------------------------------------
        
        internal void OnBeforeSerialize(Item item)
        {
            if (item.Sockets.ListLength > 0)
            {
                this.m_MaxStack = 1;
            }
        }
    }
}