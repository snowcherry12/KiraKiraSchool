using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetItemNone : PropertyTypeSetItem
    {
        public override void Set(Item value, Args args)
        { }
        
        public override void Set(Item value, GameObject gameObject)
        { }

        public static PropertySetItem Create => new PropertySetItem(
            new SetItemNone()
        );

        public override string String => "(none)";
    }
}