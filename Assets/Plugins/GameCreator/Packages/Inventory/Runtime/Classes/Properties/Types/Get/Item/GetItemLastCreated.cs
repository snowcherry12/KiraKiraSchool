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
    public class GetItemLastCreated : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Item.LastItemCreated?.Item;
        public override Item Get(GameObject gameObject) => Item.LastItemCreated?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastCreated instance = new GetItemLastCreated();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Created]";
    }
}