using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the StatusEffect value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable]
    public class SetStatusEffectLocalList : PropertyTypeSetStatusEffect
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueStatusEffect.TYPE_ID);

        public override void Set(StatusEffect value, Args args) => this.m_Variable.Set(value, args);
        public override StatusEffect Get(Args args) => this.m_Variable.Get(args) as StatusEffect;

        public static PropertySetStatusEffect Create => new PropertySetStatusEffect(
            new SetStatusEffectLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}