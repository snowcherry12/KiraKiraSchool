using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Description("Sets the FMOD Audio value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable]
    public class SetFMODAudioLocalName : PropertyTypeSetFMODAudio
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueFMODAudio.TYPE_ID);

        public override void Set(FMODAudio value, Args args) => this.m_Variable.Set(value, args);
        public override FMODAudio Get(Args args) => this.m_Variable.Get(args) as FMODAudio;

        public static PropertySetFMODAudio Create => new PropertySetFMODAudio(
            new SetFMODAudioLocalName()
        );

        public override string String => this.m_Variable.ToString();
    }
}
