using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Status Effect value of a Local List Variable")]

    [Serializable]
    public class GetStatusEffectLocalList : PropertyTypeGetStatusEffect
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueStatusEffect.TYPE_ID);

        public override StatusEffect Get(Args args) => this.m_Variable.Get<StatusEffect>(args);

        public override string String => this.m_Variable.ToString();
    }
}