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
    public class GetRuntimeItemLastInstantiated : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => Item.LastItemInstantiated;
        public override RuntimeItem Get(GameObject gameObject) => Item.LastItemInstantiated;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastInstantiated instance = new GetRuntimeItemLastInstantiated();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Instantiated]";
    }
}