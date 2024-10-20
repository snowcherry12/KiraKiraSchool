using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the FMOD Audio value of a Global Name Variable")]

    [Serializable]
    public class GetFMODAudioGlobalName : PropertyTypeGetFMODAudio
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueFMODAudio.TYPE_ID);

        public override FMODAudio Get(Args args) => this.m_Variable.Get<FMODAudio>(args);

        public override string String => this.m_Variable.ToString();
    }
}
