using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetFMODAudioNone : PropertyTypeSetFMODAudio
    {
        public override void Set(FMODAudio value, Args args)
        { }
        
        public override void Set(FMODAudio value, GameObject gameObject)
        { }

        public static PropertySetFMODAudio Create => new PropertySetFMODAudio(
            new SetFMODAudioNone()
        );

        public override string String => "(none)";
    }
}