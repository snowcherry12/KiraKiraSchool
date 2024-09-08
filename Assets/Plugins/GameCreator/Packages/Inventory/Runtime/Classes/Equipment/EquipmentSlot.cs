using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class EquipmentSlot : TPolymorphicItem<EquipmentSlot>
    {
        [SerializeField] private Item m_Base;
        [SerializeField] private HandleField m_Handle = new HandleField();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Item Base => this.m_Base;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public HandleResult Get(Args args)
        {
            return this.m_Handle.Get(args);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            return this.m_Base != null ? TextUtils.Humanize(this.m_Base.name) : "(none)";
        }
    }
}