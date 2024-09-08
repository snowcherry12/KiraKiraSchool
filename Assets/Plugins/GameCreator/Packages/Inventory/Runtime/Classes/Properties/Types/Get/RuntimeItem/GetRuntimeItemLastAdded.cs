using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Added")]
    [Category("Bags/Last Added")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Description("A reference to the last Item added to a Bag")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastAdded : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemAdded;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemAdded;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastAdded instance = new GetRuntimeItemLastAdded();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Added]";
    }
}