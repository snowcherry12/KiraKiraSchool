using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Instantiated")]
    [Category("Last Instantiated")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    [Description("A reference to the last Item instantiated on the scene")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastInstantiated : PropertyTypeGetItem
    {
        public override Item Get(Args args) => Item.LastItemInstantiated?.Item;
        public override Item Get(GameObject gameObject) => Item.LastItemInstantiated?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastInstantiated instance = new GetItemLastInstantiated();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Instantiated]";
    }
}