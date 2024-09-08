using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Add")]
    [Category("Bags/Last Attempted to Add")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Description("A reference to the last Item that was attempted to be added to a Bag, even if it was not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttemptedAdd : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemAttemptedAdd?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAttemptedAdd?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastAttemptedAdd instance = new GetItemLastAttemptedAdd();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Try Add]";
    }
}