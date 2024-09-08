using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Close Tinker UI")]
    [Description("Closes the current Tinker UI")]

    [Category("Inventory/UI/Close Tinker UI")]

    [Keywords("Craft", "Make", "Create", "Dismantle", "Disassemble", "Torn")]
    [Keywords("Alchemy", "Blacksmith")]

    [Image(typeof(IconCraft), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionInventoryCloseTinkerUI : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Close Tinker UI";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (!TinkerUI.IsOpen || TinkerUI.LastTinkerUIOpened == null) return DefaultResult;
            
            TinkerUI.LastTinkerUIOpened.gameObject.SetActive(false);
            return DefaultResult;
        }
    }
}