using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public struct TokenBagEquipment
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private IdString[] m_Equipment;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString[] Equipment
        {
            get => this.m_Equipment;
            internal set => this.m_Equipment = value;
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenBagEquipment(IBagEquipment bagEquipment)
        {
            if (bagEquipment == null)
            {
                this.m_Equipment = Array.Empty<IdString>();
                return;
            }

            int equipmentLength = bagEquipment.Count;
            this.m_Equipment = new IdString[equipmentLength];
            for (int i = 0; i < equipmentLength; ++i)
            {
                this.m_Equipment[i] = bagEquipment.GetSlotRootRuntimeItemID(i);
            }
        }
    }
}