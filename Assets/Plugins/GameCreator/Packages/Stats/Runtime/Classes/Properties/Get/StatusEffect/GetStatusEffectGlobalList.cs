using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Status Effect value of a Global List Variable")]

    [Serializable]
    public class GetStatusEffectGlobalList : PropertyTypeGetStatusEffect
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueStatusEffect.TYPE_ID);

        public override StatusEffect Get(Args args) => this.m_Variable.Get<StatusEffect>(args);

        public override string String => this.m_Variable.ToString();
    }
}