using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]

    [Description("Sets the FMOD Audio value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable]
    public class SetFMODAudioLocalList : PropertyTypeSetFMODAudio
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueFMODAudio.TYPE_ID);

        public override void Set(FMODAudio value, Args args) => this.m_Variable.Set(value, args);
        public override FMODAudio Get(Args args) => this.m_Variable.Get(args) as FMODAudio;

        public static PropertySetFMODAudio Create => new PropertySetFMODAudio(
            new SetFMODAudioLocalList()
        );

        public override string String => this.m_Variable.ToString();
    }
}
