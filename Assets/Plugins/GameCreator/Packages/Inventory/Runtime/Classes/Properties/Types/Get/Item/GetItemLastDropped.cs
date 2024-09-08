using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Dropped")]
    [Category("Bags/Last Dropped")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow, typeof(OverlayArrowDown))]
    [Description("A reference to the last Item dropped from a Bag on the scene")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastDropped : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Item.LastItemDropped?.Item;
        public override Item Get(GameObject gameObject) => Item.LastItemDropped?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastDropped instance = new GetItemLastDropped();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Dropped]";
    }
}