using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("Is Bag UI Open")]
    [Description("Returns true if the there is a Bag UI open")]

    [Category("Inventory/UI/Is Bag UI Open")]

    [Keywords("Inventory", "Close", "Stash", "Loot", "Container", "Chest")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionInventoryIsOpenBagUI : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => "Bag UI is Open";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return TBagUI.IsOpen;
        }
    }
}
