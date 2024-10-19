using System;
using UnityEngine;
using FMODUnity;

namespace GameCreator.Runtime.Common
{
    [Title("None")]
    [Category("None")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]
    [Description("Returns a empty FMOD Audio")]

    [Serializable] [HideLabelsInEditor]
    public class GetFMODAudioNone : PropertyTypeGetFMODAudio
    {
        public override FMODAudio Get(Args args) => null;
        public override FMODAudio Get(GameObject gameObject) => null;

        public static PropertyGetFMODAudio Create => new (
            new GetFMODAudioNone()
        );

        public override string String => "None";
    }
}