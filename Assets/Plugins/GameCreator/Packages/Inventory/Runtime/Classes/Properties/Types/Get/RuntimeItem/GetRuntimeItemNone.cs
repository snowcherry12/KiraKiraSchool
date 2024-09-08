using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("None")]
    [Category("None")]
    
    [Image(typeof(IconEmpty), ColorTheme.Type.TextLight)]
    [Description("An empty reference to no Runtime Item")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemNone : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => null;
        public override RuntimeItem Get(GameObject gameObject) => null;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemNone instance = new GetRuntimeItemNone();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "None";
    }
}