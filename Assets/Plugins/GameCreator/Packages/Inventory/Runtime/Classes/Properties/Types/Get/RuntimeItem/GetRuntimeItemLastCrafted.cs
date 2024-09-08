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
    public class GetRuntimeItemLastCrafted : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Crafting.LastItemCrafted;
        public override RuntimeItem Get(GameObject gameObject) => Crafting.LastItemCrafted;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastCrafted instance = new GetRuntimeItemLastCrafted();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Crafted]";
    }
}