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
    public class SetStatNone : PropertyTypeSetStat
    {
        public override void Set(Stat value, Args args)
        { }
        
        public override void Set(Stat value, GameObject gameObject)
        { }

        public static PropertySetStat Create => new PropertySetStat(
            new SetStatNone()
        );

        public override string String => "(none)";
    }
}