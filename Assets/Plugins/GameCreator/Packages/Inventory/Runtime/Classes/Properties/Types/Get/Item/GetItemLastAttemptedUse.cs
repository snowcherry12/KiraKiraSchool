using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Use")]
    [Category("Bags/Last Attempted to Use")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("A reference to the last Item that was attempted to be used, even if it was not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttemptedUse : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemAttemptedUse?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAttemptedUse?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastAttemptedUse instance = new GetItemLastAttemptedUse();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Try Use]";
    }
}