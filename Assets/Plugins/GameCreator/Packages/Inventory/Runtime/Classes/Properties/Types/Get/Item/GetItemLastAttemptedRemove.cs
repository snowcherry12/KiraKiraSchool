using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Remove")]
    [Category("Bags/Last Attempted to Remove")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("A reference to the last Item that was attempted to be removed from a Bag, even if it was not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttemptedRemove : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemAttemptedRemove?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAttemptedRemove?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastAttemptedRemove instance = new GetItemLastAttemptedRemove();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Try Remove]";
    }
}