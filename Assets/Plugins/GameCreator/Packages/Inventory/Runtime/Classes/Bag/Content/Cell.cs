using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Cell
    {
        [Serializable]
        private class Stack : TSerializableLinkList<RuntimeItem>
        { }

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Stack m_Stack = new Stack();
        [SerializeField] private RuntimeItem m_RuntimeItem;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public int Count => this.m_RuntimeItem != null ? this.m_Stack.Count + 1 : 0;

        public int CountRemaining => this.m_RuntimeItem != null
            ? this.m_RuntimeItem.Item.Shape.MaxStack - this.Count
            : 0;

        public Item Item => this.m_RuntimeItem?.Item;

        public RuntimeItem RootRuntimeItem => this.m_RuntimeItem;
        public IdString RootRuntimeItemID => this.m_RuntimeItem?.RuntimeID ?? default;
        
        public bool Available => this.m_RuntimeItem == null;

        public int Weight => this.m_RuntimeItem != null 
            ? this.m_RuntimeItem.Weight * this.Count 
            : 0;

        public List<IdString> List
        {
            get
            {
                List<IdString> list = new List<IdString>();
                if (this.Available) return list;
                
                list.Add(this.RootRuntimeItemID);
                foreach (RuntimeItem element in this.m_Stack)
                {
                    if (element == null) continue;
                    list.Add(element.RuntimeID);
                }

                return list;
            }
        }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Cell()
        { }

        public Cell(RuntimeItem runtimeItem) : this()
        {
            this.m_RuntimeItem = runtimeItem;
            this.m_Stack = new Stack();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Contains(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return false;
            if (this.m_RuntimeItem == null) return false;
            
            if (this.m_RuntimeItem.RuntimeID.Hash == runtimeItem.RuntimeID.Hash) return true;
            return this.m_Stack.Contains(runtimeItem);
        }

        public bool ContainsType(IdString itemID)
        {
            return this.m_RuntimeItem != null && this.m_RuntimeItem.InheritsFrom(itemID);
        }
        
        public bool CanStack(RuntimeItem runtimeItem, int amount = 1)
        {
            if (runtimeItem == null) return false;
            if (this.m_RuntimeItem == null) return false;
            if (!this.m_RuntimeItem.CanStack(runtimeItem)) return false;
            
            return this.Count + amount <= this.m_RuntimeItem.Item.Shape.MaxStack;
        }

        public RuntimeItem Get(IdString runtimeItemID)
        {
            if (this.RootRuntimeItemID.Hash == runtimeItemID.Hash) return this.m_RuntimeItem;
            foreach (RuntimeItem stackRuntimeItem in this.m_Stack)
            {
                if (stackRuntimeItem.RuntimeID.Hash != runtimeItemID.Hash) continue;
                return stackRuntimeItem;
            }

            return null;
        }

        public bool Add(RuntimeItem runtimeItem, bool canStack)
        {
            if (this.m_RuntimeItem == null)
            {
                this.m_RuntimeItem = runtimeItem;
                this.m_Stack = new Stack();
                return true;
            }
            
            if (canStack && this.CanStack(runtimeItem))
            {
                this.m_Stack.AddLast(runtimeItem);
                return true;
            }

            return false;
        }

        public RuntimeItem Peek()
        {
            if (this.m_RuntimeItem == null) return null;
            return this.m_Stack.Count == 0
                ? this.m_RuntimeItem 
                : this.m_Stack.Last();
        }

        public RuntimeItem Pop()
        {
            if (this.m_RuntimeItem == null) return null;

            if (this.m_Stack.Count == 0)
            {
                RuntimeItem result = this.m_RuntimeItem;
                this.m_RuntimeItem = null;
                return result;
            }

            return this.m_Stack.RemoveLast();
        }

        public RuntimeItem Remove(IdString runtimeItemID)
        {
            if (this.m_RuntimeItem == null) return null;
            if (string.IsNullOrEmpty(runtimeItemID.String)) return null;

            if (this.m_RuntimeItem.RuntimeID.Hash == runtimeItemID.Hash)
            {
                RuntimeItem result = this.m_RuntimeItem;

                this.m_RuntimeItem = this.m_Stack.IsEmpty ? null : this.m_Stack.RemoveFirst();
                return result;
            }

            foreach (RuntimeItem stackRuntimeItem in this.m_Stack)
            {
                if (stackRuntimeItem.RuntimeID.Hash != runtimeItemID.Hash) continue;
                this.m_Stack.Remove(stackRuntimeItem);
                return stackRuntimeItem;
            }

            return null;
        }
    }
}