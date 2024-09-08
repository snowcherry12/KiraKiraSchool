using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class EquipmentRuntime : TSerializableDictionary<int, EquipmentRuntimeSlot>
    {
        internal void SyncWithEquipment(Equipment equipment)
        {
            if (equipment == null)
            {
                this.m_Dictionary.Clear();
                return;
            }
            
            Dictionary<int, EquipmentRuntimeSlot> data = new Dictionary<int, EquipmentRuntimeSlot>();

            for (int i = 0; i < equipment.Slots.Length; ++i)
            {
                EquipmentSlot slot = equipment.Slots[i];
                if (slot == null) continue;

                this.m_Dictionary.TryGetValue(i, out EquipmentRuntimeSlot entry);
                data[i] = entry ?? new EquipmentRuntimeSlot();
            }

            this.m_Dictionary = data;
        }
    }
}