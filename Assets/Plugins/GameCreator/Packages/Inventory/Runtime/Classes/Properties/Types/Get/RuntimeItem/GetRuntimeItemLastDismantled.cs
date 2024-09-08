using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Dismantled")]
    [Category("Tinker/Last Dismantled")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Red)]
    [Description("A reference to the last Item dismantled")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastDismantled : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Crafting.LastItemDismantled;
        public override RuntimeItem Get(GameObject gameObject) => Crafting.LastItemDismantled;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastDismantled instance = new GetRuntimeItemLastDismantled();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Dismantled]";
    }
}