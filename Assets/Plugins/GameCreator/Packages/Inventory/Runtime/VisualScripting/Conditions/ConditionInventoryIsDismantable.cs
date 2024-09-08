using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Dismantable")]
    [Description("Returns true if the chosen Item can be dismantled")]

    [Category("Inventory/Tinker/Is Dismantable")]
    
    [Parameter("Item", "The item type to check")]
    [Keywords("Inventory", "Destroy", "Tear", "Break")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionInventoryIsDismantable : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Item} dismantable";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null && item.Crafting.AllowToDismantle;
        }
    }
}
