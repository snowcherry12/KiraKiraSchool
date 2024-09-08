using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Unequip Item")]
    [Description("Unequip an Item from the Bag that inherits from the specified type")]

    [Category("Inventory/Equipment/Unequip Item")]
    
    [Parameter("Item", "The parent type of item to equip")]
    [Parameter("Bag", "The targeted Bag component")]

    [Keywords("Bag", "Inventory", "Equipment")]
    [Keywords("Take", "Sheathe", "Inventory", "Remove")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Green, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionInventoryUnequipItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Unequip {this.m_Item} from {this.m_Bag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            if (item == null) return;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;

            Cell[] cells = bag.Content.GetTypes(item);
            foreach (Cell cell in cells)
            {
                if (cell == null || cell.Available) continue;
                List<IdString> cellRuntimeItemsID = cell.List;
                foreach (IdString cellRuntimeItemID in cellRuntimeItemsID)
                {
                    RuntimeItem cellRuntimeItem = bag.Content.GetRuntimeItem(cellRuntimeItemID);
                    if (cellRuntimeItem == null) continue;

                    if (bag.Equipment.IsEquipped(cellRuntimeItem))
                    {
                        bool didUnequip = await bag.Equipment.Unequip(cellRuntimeItem);
                        if (didUnequip) return;
                    }
                }
            }
        }
    }
}