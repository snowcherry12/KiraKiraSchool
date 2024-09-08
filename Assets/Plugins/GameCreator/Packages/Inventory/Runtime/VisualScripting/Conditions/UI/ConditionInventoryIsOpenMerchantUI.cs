using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("Is Merchant UI Open")]
    [Description("Returns true if the there is a Merchant UI open")]

    [Category("Inventory/UI/Is Merchant UI Open")]

    [Keywords("Shop", "Exchange", "Trader")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionInventoryIsOpenMerchantUI : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => "Merchant UI is Open";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return MerchantUI.IsOpen;
        }
    }
}
