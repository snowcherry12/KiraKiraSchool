using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the FMOD Audio value of a Local Name Variable")]

    [Serializable]
    public class GetFMODAudioLocalName : PropertyTypeGetFMODAudio
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueFMODAudio.TYPE_ID);

        public override FMODAudio Get(Args args) => this.m_Variable.Get<FMODAudio>(args);

        public override string String => this.m_Variable.ToString();
    }
}
