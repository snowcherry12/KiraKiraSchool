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
    public class GetRuntimeItemLastBought : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Merchant.LastItemSold;
        public override RuntimeItem Get(GameObject gameObject) => Merchant.LastItemSold;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastBought instance = new GetRuntimeItemLastBought();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Bought]";
    }
}