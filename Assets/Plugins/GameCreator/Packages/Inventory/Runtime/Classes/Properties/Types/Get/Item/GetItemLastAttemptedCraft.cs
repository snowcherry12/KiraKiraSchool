using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Craft")]
    [Category("Tinker/Last Attempted to Craft")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("A reference to the last Item attempted to be crafted, even if not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttemptedCraft : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Crafting.LastItemAttemptedCraft;
        public override Item Get(GameObject gameObject) => Crafting.LastItemAttemptedCraft;

        public static PropertyGetItem Create()
        {
            GetItemLastAttemptedCraft instance = new GetItemLastAttemptedCraft();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Try Craft]";
    }
}