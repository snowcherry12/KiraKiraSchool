using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]

    [Description("Sets the FMOD Audio value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable]
    public class SetFMODAudioGlobalName : PropertyTypeSetFMODAudio
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueFMODAudio.TYPE_ID);

        public override void Set(FMODAudio value, Args args) => this.m_Variable.Set(value, args);
        public override FMODAudio Get(Args args) => this.m_Variable.Get(args) as FMODAudio;

        public static PropertySetFMODAudio Create => new PropertySetFMODAudio(
            new SetFMODAudioGlobalName()
        );

        public override string String => this.m_Variable.ToString();
    }
}
