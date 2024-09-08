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
    public class GetItemLastDismantled : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Crafting.LastItemDismantled?.Item;
        public override Item Get(GameObject gameObject) => Crafting.LastItemDismantled?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastDismantled instance = new GetItemLastDismantled();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Dismantled]";
    }
}