using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Equip Runtime Item")]
    [Description("Equips the specified Runtime Item")]

    [Category("Inventory/Equipment/Equip Runtime Item")]
    
    [Parameter("Runtime Item", "The item instance to equip")]

    [Keywords("Bag", "Inventory", "Equipment")]
    [Keywords("Put", "Wear", "Inventory", "Wield")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionInventoryEquipRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Equip {this.m_RuntimeItem}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Item == null) return;

            Bag bag = runtimeItem.Bag;
            if (bag == null) return;
            
            await bag.Equipment.Equip(runtimeItem);
        }
    }
}