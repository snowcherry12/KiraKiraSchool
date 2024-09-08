using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Bought")]
    [Category("Merchant/Last Bought")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayArrowRight))]
    [Description("A reference to the last Item bought from a Merchant")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastBought : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Merchant.LastItemBought?.Item;
        public override Item Get(GameObject gameObject) => Merchant.LastItemBought?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastBought instance = new GetItemLastBought();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Bought]";
    }
}