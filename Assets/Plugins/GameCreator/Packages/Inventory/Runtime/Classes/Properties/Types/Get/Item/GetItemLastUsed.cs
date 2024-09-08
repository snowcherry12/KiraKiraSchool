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
    public class GetItemLastUsed : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemUsed?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemUsed?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastUsed instance = new GetItemLastUsed();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Used]";
    }
}