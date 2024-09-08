using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Used")]
    [Category("Bags/Last Used")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow)]
    [Description("A reference to the last Item used")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastUsed : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemUsed;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemUsed;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastUsed instance = new GetRuntimeItemLastUsed();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Used]";
    }
}