using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item has Property")]
    [Description("Returns true if the chosen Item has the specified item Property")]

    [Category("Inventory/Properties/Item has Property")]
    
    [Parameter("Item", "The item type to check")]
    [Parameter("Property", "The item property")]
    
    [Keywords("Inventory", "Contains", "Exists")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green, typeof(OverlayDot))]
    
    [Serializable]
    public class ConditionInventoryItemHasProperty : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"does {this.m_Item} have {this.m_PropertyId}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null && item.Properties.Get(this.m_PropertyId, item) != null;
        }
    }
}
