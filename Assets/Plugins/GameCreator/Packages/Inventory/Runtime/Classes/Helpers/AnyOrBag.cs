using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class AnyOrBag
    {
        private enum Option
        {
            Any = 0,
            ByBag = 1
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Option m_Option = Option.Any;

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryTBagUI.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool Any => this.m_Option == Option.Any;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public AnyOrBag()
        { }

        public AnyOrBag(PropertyGetGameObject property) : this()
        {
            this.m_Option = Option.ByBag;
            this.m_Bag = property;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(Bag compareBag, Args args)
        {
            if (this.Any) return true;
            if (compareBag == null) return false;
            
            return compareBag == this.Get(args);
        }
        
        public bool Match(Bag compareBag, GameObject args)
        {
            if (this.Any) return true;
            if (compareBag == null) return false;
            
            return compareBag == this.Get(args);
        }
        
        public Bag Get(Args args)
        {
            return this.m_Bag.Get<Bag>(args);
        }

        public Bag Get(GameObject target)
        {
            return this.m_Bag.Get<Bag>(target);
        }
    }
}
