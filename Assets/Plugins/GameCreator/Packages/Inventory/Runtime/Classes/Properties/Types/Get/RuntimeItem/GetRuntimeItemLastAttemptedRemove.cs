using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Remove")]
    [Category("Bags/Last Attempted to Remove")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("A reference to the last Item attempted to be removed, even if it was not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastAttemptedRemove : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemAttemptedRemove;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAttemptedRemove;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastAttemptedRemove instance = new GetRuntimeItemLastAttemptedRemove();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Try Remove]";
    }
}