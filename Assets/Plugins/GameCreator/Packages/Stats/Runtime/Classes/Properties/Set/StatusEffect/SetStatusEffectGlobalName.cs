using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the StatusEffect value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable]
    public class SetStatusEffectGlobalName : PropertyTypeSetStatusEffect
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueStatusEffect.TYPE_ID);

        public override void Set(StatusEffect value, Args args) => this.m_Variable.Set(value, args);
        public override StatusEffect Get(Args args) => this.m_Variable.Get(args) as StatusEffect;

        public static PropertySetStatusEffect Create => new PropertySetStatusEffect(
            new SetStatusEffectGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}