using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Crafted")]
    [Category("Tinker/Last Crafted")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Teal)]
    [Description("A reference to the last Item crafted")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastCrafted : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Crafting.LastItemCrafted?.Item;
        public override Item Get(GameObject gameObject) => Crafting.LastItemCrafted?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastCrafted instance = new GetItemLastCrafted();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Crafted]";
    }
}