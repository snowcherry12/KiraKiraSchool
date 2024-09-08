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
    public class GetRuntimeItemLastSold : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Merchant.LastItemBought;
        public override RuntimeItem Get(GameObject gameObject) => Merchant.LastItemBought;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastSold instance = new GetRuntimeItemLastSold();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Sold]";
    }
}