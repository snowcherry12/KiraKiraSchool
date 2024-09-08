using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Equipment",
        menuName = "Game Creator/Inventory/Equipment"
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoEquipment.png")]
    [Serializable]
    public class Equipment : ScriptableObject
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private EquipmentSlots m_Slots = new EquipmentSlots();

        // PROPERTIES: ----------------------------------------------------------------------------

        public EquipmentSlot[] Slots => m_Slots.Slots;
    }
}
