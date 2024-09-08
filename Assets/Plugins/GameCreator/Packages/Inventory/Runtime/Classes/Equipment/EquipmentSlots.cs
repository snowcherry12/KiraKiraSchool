using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class EquipmentSlots
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private EquipmentSlot[] m_Slots = Array.Empty<EquipmentSlot>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public EquipmentSlot[] Slots => m_Slots;
    }
}