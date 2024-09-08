using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Equip Item")]
    [Description("Equips an Item from the Bag that inherits from the specified type")]

    [Category("Inventory/Equipment/Equip Item")]
    
    [Parameter("Item", "The parent type of item to equip")]
    [Parameter("Bag", "The targeted Bag component")]

    [Keywords("Bag", "Inventory", "Equipment")]
    [Keywords("Put", "Wear", "Inventory", "Wield")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Green, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionInventoryEquipItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Equip {this.m_Item} from {this.m_Bag}";

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
                if (await bag.Equipment.Equip(cell.Peek())) return;
            }
        }
    }
}