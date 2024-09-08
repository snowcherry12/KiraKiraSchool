using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Coins : TPolymorphicList<Coin>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] private Coin[] m_List = { new Coin("Guiles", 1) };
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Coin this[int index] => this.m_List[index];
        public override int Length => this.m_List.Length;

        private int[] SortIndexes
        {
            get
            {
                KeyValuePair<int, int>[] indexes = new KeyValuePair<int, int>[this.Length];

                for (int i = 0; i < this.Length; ++i)
                {
                    indexes[i] = new KeyValuePair<int, int>(i, this[i].Multiplier);
                }
                
                Array.Sort(indexes, (a, b) => b.Value.CompareTo(a.Value));
                
                int[] sort = new int[this.Length];
                for (int i = 0; i < indexes.Length; ++i) sort[i] = indexes[i].Key;

                return sort;
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public int[] Values(int amount)
        {
            int[] conversion = new int[this.Length];
            int[] indexes = this.SortIndexes;
            
            for (int i = 0; i < this.Length; ++i)
            {
                int index = indexes[i];
                conversion[index] = amount / this[index].Multiplier;
                amount %= this[index].Multiplier;
            }

            return conversion;
        }
    }
}