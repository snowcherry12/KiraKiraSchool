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
    public class SetRuntimeItemNone : PropertyTypeSetRuntimeItem
    {
        public override void Set(RuntimeItem value, Args args)
        { }
        
        public override void Set(RuntimeItem value, GameObject gameObject)
        { }

        public static PropertySetRuntimeItem Create => new PropertySetRuntimeItem(
            new SetRuntimeItemNone()
        );

        public override string String => "(none)";
    }
}