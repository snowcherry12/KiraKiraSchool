using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Removed")]
    [Category("Bags/Last Removed")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("A reference to the last Item removed from a Bag")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastRemoved : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemRemoved;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemRemoved;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastRemoved instance = new GetRuntimeItemLastRemoved();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Removed]";
    }
}