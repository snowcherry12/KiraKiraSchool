using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Dismantle")]
    [Category("Tinker/Last Attempted to Dismantle")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Red, typeof(OverlayDot))]
    [Description("A reference to the last Item attempted to be dismantled, even if not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttemptedDismantle : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Crafting.LastItemAttemptedDismantle;
        public override Item Get(GameObject gameObject) => Crafting.LastItemAttemptedDismantle;

        public static PropertyGetItem Create()
        {
            GetItemLastAttemptedDismantle instance = new GetItemLastAttemptedDismantle();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Try Dismantle]";
    }
}