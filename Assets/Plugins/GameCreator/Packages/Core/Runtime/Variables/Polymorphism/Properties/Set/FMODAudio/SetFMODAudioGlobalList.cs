using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]

    [Description("Sets the FMOD Audio value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable]
    public class SetFMODAudioGlobalList : PropertyTypeSetFMODAudio
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueFMODAudio.TYPE_ID);

        public override void Set(FMODAudio value, Args args) => this.m_Variable.Set(value, args);
        public override FMODAudio Get(Args args) => this.m_Variable.Get(args) as FMODAudio;

        public static PropertySetFMODAudio Create => new PropertySetFMODAudio(
            new SetFMODAudioGlobalList()
        );

        public override string String => this.m_Variable.ToString();
    }
}
