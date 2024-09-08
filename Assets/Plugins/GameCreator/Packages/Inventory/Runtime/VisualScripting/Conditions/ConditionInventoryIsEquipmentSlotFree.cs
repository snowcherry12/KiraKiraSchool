using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Equipment Slot Free")]
    [Description("Returns true if the Bag's equipment slot does not have any Item assigned")]

    [Category("Inventory/Equipment/Is Equipment Slot Free")]
    
    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Equipment Slot", "The Equipment slot to check")]

    [Keywords("Inventory", "Wears", "Slot", "Hotbar")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class ConditionInventoryIsEquipmentSlotFree : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private EquipmentIndex m_EquipmentIndex = new EquipmentIndex();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_EquipmentIndex} available";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return false;

            int index = this.m_EquipmentIndex.Index;
            if (index < 0) return false;

            IdString runtimeItemID = bag.Equipment.GetSlotRootRuntimeItemID(index);
            return string.IsNullOrEmpty(runtimeItemID.String);
        }
    }
}
