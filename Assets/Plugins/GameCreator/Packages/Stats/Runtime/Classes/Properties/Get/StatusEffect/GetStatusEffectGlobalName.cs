using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Status Effect value of a Global Name Variable")]

    [Serializable]
    public class GetStatusEffectGlobalName : PropertyTypeGetStatusEffect
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueStatusEffect.TYPE_ID);

        public override StatusEffect Get(Args args) => this.m_Variable.Get<StatusEffect>(args);

        public override string String => this.m_Variable.ToString();
    }
}