using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class EquipmentIndex
    {
        [SerializeField] private Equipment m_Equipment;
        [SerializeField] private int m_Index;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Index => this.m_Index;
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            return this.m_Equipment != null && this.m_Index < this.m_Equipment.Slots.Length
                ? this.m_Equipment.Slots[this.m_Index].ToString() 
                : "(none)";
        }
    }
}