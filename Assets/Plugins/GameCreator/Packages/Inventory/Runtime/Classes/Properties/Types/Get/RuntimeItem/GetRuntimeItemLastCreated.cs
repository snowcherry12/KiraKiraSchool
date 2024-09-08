using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Created")]
    [Category("Last Created")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("A reference to the last Item created")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastCreated : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Item.LastItemCreated;
        public override RuntimeItem Get(GameObject gameObject) => Item.LastItemCreated;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastCreated instance = new GetRuntimeItemLastCreated();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Created]";
    }
}