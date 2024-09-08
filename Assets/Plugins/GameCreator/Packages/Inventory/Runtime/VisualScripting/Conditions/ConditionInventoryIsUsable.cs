using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Usable")]
    [Description("Returns true if the chosen Item can be used")]

    [Category("Inventory/Is Usable")]
    
    [Parameter("Item", "The item type to check")]
    [Keywords("Inventory", "Consume", "Drink")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionInventoryIsUsable : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Item} usable";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null && item.Usage.AllowUse;
        }
    }
}
