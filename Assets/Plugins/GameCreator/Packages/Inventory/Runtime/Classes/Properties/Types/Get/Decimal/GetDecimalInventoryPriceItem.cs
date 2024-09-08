using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Price")]
    [Category("Inventory/Item Price")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("The price of an Item")]

    [Parameter("Item", "The Item type")]
    
    [Keywords("Inventory", "Amount", "Total")]

    [Serializable]
    public class GetDecimalInventoryPriceItem : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetItem m_Item = GetItemInstance.Create();

        public override double Get(Args args)
        {
            Item item = this.m_Item.Get(args);
            return item != null ? item.Price.ValueRaw : 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryPriceItem()
        );

        public override string String => $"{this.m_Item} Price";
    }
}