using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Added")]
    [Category("Bags/Last Added")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Description("A reference to the last Item added to a Bag")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAdded : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemAdded?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAdded?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastAdded instance = new GetItemLastAdded();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Added]";
    }
}