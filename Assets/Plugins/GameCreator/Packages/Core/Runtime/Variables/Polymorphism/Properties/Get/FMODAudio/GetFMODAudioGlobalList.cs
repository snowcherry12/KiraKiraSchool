using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]

    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the FMOD Audio value of a Global List Variable")]

    [Serializable]
    public class GetFMODAudioGlobalList : PropertyTypeGetFMODAudio
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueFMODAudio.TYPE_ID);

        public override FMODAudio Get(Args args) => this.m_Variable.Get<FMODAudio>(args);

        public override string String => this.m_Variable.ToString();
    }
}
