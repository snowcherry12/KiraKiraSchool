using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Type of Item")]
    [Description("Returns true if the item is equal or a sub-type of another one")]

    [Category("Inventory/Is Type of Item")]
    
    [Parameter("Item", "The item source")]
    [Parameter("Compare To", "The item compared to")]

    [Keywords("Inventory", "Compare")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow)]
    [Serializable]
    public class ConditionInventoryItemType : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] protected PropertyGetItem m_CompareTo = new PropertyGetItem();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"item {this.m_Item} is {this.m_CompareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            Item compareTo = this.m_CompareTo.Get(args);

            if (item == null) return false;
            return compareTo != null && item.InheritsFrom(compareTo.ID);
        }
    }
}
