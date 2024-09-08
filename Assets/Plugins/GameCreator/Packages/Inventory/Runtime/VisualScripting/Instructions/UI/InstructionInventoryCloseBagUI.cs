using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Close Bag UI")]
    [Description("Closes the current inventory UI")]

    [Category("Inventory/UI/Close Bag UI")]

    [Keywords("Item", "Inventory", "Catalogue", "Content", "Sort")]
    [Keywords("Equipment", "Hotbar", "Consume")]

    [Image(typeof(IconBagOutline), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionInventoryCloseBagUI : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Close Bag UI";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (!TBagUI.IsOpen || TBagUI.LastBagUIOpened == null) return DefaultResult;
            
            TBagUI.LastBagUIOpened.gameObject.SetActive(false);
            return DefaultResult;
        }
    }
}