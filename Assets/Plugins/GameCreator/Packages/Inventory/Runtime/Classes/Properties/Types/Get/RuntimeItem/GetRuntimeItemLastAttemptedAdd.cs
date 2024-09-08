using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Add")]
    [Category("Bags/Last Attempted to Add")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Description("A reference to the last Item attempted to be added, even if it was not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastAttemptedAdd : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemAttemptedAdd;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAttemptedAdd;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastAttemptedAdd instance = new GetRuntimeItemLastAttemptedAdd();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Try Add]";
    }
}