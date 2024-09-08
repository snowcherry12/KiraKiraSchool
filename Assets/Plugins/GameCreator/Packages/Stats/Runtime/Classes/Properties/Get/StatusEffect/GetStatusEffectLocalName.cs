using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Status Effect value of a Local Name Variable")]
    
    [Serializable]
    public class GetStatusEffectLocalName : PropertyTypeGetStatusEffect
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueStatusEffect.TYPE_ID);

        public override StatusEffect Get(Args args) => this.m_Variable.Get<StatusEffect>(args);

        public override string String => this.m_Variable.ToString();
    }
}