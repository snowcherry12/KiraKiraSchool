using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Runtime Item Equipped")]
    [Description("Returns true if the Bag's wearer has the Runtime Item currently equipped")]

    [Category("Inventory/Equipment/Is Runtime Item Equipped")]
    
    [Parameter("Runtime Item", "The Runtime Item to check")]

    [Keywords("Inventory", "Wears")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionInventoryIsEquippedRuntimeItem : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_RuntimeItem} Equipped";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            return runtimeItem.Bag != null && runtimeItem.Bag.Equipment.IsEquipped(runtimeItem);
        }
    }
}
