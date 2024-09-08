using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class AnyOrItem
    {
        private enum Option
        {
            Any = 0,
            ByType = 1
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Option m_Option = Option.Any;
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool Any => this.m_Option == Option.Any;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public AnyOrItem()
        { }

        public AnyOrItem(PropertyGetItem property) : this()
        {
            this.m_Option = Option.ByType;
            this.m_Item = property;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(Item compareItem, Args args)
        {
            if (this.Any) return true;
            return compareItem == this.Get(args);
        }
        
        public bool Match(Item compareItem, GameObject args)
        {
            if (this.Any) return true;
            if (compareItem == null) return false;
            
            Item type = this.Get(args);
            return type == null || compareItem.InheritsFrom(type.ID);
        }
        
        public Item Get(Args args)
        {
            return this.m_Item.Get(args);
        }

        public Item Get(GameObject target)
        {
            return this.m_Item.Get(target);
        }
    }
}
