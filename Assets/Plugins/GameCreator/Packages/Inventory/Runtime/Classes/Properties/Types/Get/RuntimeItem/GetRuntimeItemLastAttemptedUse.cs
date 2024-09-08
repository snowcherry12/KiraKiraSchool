using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attempted to Use")]
    [Category("Bags/Last Attempted to Use")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("A reference to the last Item attempted to be used, even if it was not successful")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastAttemptedUse : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemAttemptedUse;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAttemptedUse;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastAttemptedUse instance = new GetRuntimeItemLastAttemptedUse();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Try Use]";
    }
}