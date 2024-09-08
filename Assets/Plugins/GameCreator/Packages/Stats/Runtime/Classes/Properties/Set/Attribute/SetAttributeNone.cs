using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetAttributeNone : PropertyTypeSetAttribute
    {
        public override void Set(Attribute value, Args args)
        { }
        
        public override void Set(Attribute value, GameObject gameObject)
        { }

        public static PropertySetAttribute Create => new PropertySetAttribute(
            new SetAttributeNone()
        );

        public override string String => "(none)";
    }
}