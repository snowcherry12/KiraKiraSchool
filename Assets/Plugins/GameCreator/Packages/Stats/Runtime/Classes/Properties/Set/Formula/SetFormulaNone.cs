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
    public class SetFormulaNone : PropertyTypeSetFormula
    {
        public override void Set(Formula value, Args args)
        { }
        
        public override void Set(Formula value, GameObject gameObject)
        { }

        public static PropertySetFormula Create => new PropertySetFormula(
            new SetFormulaNone()
        );

        public override string String => "(none)";
    }
}