using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Close Merchant UI")]
    [Description("Closes the current Merchant UI")]

    [Category("Inventory/UI/Close Merchant UI")]

    [Keywords("Trade", "Merchant", "Shop", "Buy", "Sell", "Junk")]

    [Image(typeof(IconMerchant), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionInventoryCloseMerchantUI : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Close Merchant UI";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (!MerchantUI.IsOpen || MerchantUI.LastMerchantUIOpened == null) return DefaultResult;
            
            MerchantUI.LastMerchantUIOpened.gameObject.SetActive(false);
            return DefaultResult;
        }
    }
}