using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("None")]
    [Category("None")]
    
    [Image(typeof(IconEmpty), ColorTheme.Type.TextLight)]
    [Description("An empty reference to no Item")]

    [Serializable]
    public class GetItemNone : PropertyTypeGetItem
    {
        public override Item Get(Args args) => null;
        public override Item Get(GameObject gameObject) => null;

        public static PropertyGetItem Create()
        {
            GetItemNone instance = new GetItemNone();
            return new PropertyGetItem(instance);
        }

        public override string String => "None";
    }
}