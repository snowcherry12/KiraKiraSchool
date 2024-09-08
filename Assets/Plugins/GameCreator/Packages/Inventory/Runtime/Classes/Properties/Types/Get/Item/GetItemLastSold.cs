using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Sold")]
    [Category("Merchant/Last Sold")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayArrowLeft))]
    [Description("A reference to the last Item sold to a Merchant")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastSold : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Merchant.LastItemSold?.Item;
        public override Item Get(GameObject gameObject) => Merchant.LastItemSold?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastSold instance = new GetItemLastSold();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Sold]";
    }
}