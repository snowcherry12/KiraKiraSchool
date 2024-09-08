using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the StatusEffect value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable]
    public class SetStatusEffectLocalName : PropertyTypeSetStatusEffect
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueStatusEffect.TYPE_ID);

        public override void Set(StatusEffect value, Args args) => this.m_Variable.Set(value, args);
        public override StatusEffect Get(Args args) => this.m_Variable.Get(args) as StatusEffect;

        public static PropertySetStatusEffect Create => new PropertySetStatusEffect(
            new SetStatusEffectLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}