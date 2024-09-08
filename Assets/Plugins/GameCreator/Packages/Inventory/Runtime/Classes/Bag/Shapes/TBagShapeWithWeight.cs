using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public abstract class TBagShapeWithWeight : TBagShape
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private bool m_HasMaxWeight = false;
        [SerializeField] private PropertyGetInteger m_MaxWeight = new PropertyGetInteger(150);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool HasMaxWeight => this.m_HasMaxWeight;

        public override int MaxWeight => this.m_HasMaxWeight 
            ? (int) Math.Floor(this.m_MaxWeight.Get(this.Bag.Args)) 
            : int.MaxValue;
    }
}