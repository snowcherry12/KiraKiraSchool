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
    public class GetItemLastRemoved : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemRemoved?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemRemoved?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastRemoved instance = new GetItemLastRemoved();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Removed]";
    }
}