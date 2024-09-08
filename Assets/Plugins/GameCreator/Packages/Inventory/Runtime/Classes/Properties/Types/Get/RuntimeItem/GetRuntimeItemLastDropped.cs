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
    public class GetRuntimeItemLastDropped : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Item.LastItemDropped;
        public override RuntimeItem Get(GameObject gameObject) => Item.LastItemDropped;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastDropped instance = new GetRuntimeItemLastDropped();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Dropped]";
    }
}