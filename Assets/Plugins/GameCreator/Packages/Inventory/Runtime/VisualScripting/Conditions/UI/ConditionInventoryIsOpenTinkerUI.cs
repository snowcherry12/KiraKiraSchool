using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("Is Tinker UI Open")]
    [Description("Returns true if the there is a Crafting/Dismantling UI open")]

    [Category("Inventory/UI/Is Tinker UI Open")]

    [Keywords("Close", "Craft", "Dismantle", "Assemble", "Disassemble", "Smith", "Upgrade")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionInventoryIsOpenTinkerUI : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => "Tinker UI is Open";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return TinkerUI.IsOpen;
        }
    }
}
