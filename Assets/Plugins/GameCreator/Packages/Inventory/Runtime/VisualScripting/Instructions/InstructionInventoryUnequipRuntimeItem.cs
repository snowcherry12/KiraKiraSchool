using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Unequip Runtime Item")]
    [Description("Unequip an Item instance that is currently equipped")]

    [Category("Inventory/Equipment/Unequip Runtime Item")]
    
    [Parameter("Runtime Item", "The Item instance to unequip")]

    [Keywords("Bag", "Inventory", "Equipment")]
    [Keywords("Take", "Sheathe", "Inventory", "Remove")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionInventoryUnequipRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Unequip {this.m_RuntimeItem}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Item == null) return;

            Bag bag = runtimeItem.Bag;
            if (bag == null) return;

            if (bag.Equipment.IsEquipped(runtimeItem))
            {
                await bag.Equipment.Unequip(runtimeItem);
            }
        }
    }
}