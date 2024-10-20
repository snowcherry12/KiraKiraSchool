using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]

    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the FMOD Audio value of a Local List Variable")]

    [Serializable]
    public class GetFMODAudioLocalList : PropertyTypeGetFMODAudio
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueFMODAudio.TYPE_ID);

        public override FMODAudio Get(Args args) => this.m_Variable.Get<FMODAudio>(args);

        public override string String => this.m_Variable.ToString();
    }
}
