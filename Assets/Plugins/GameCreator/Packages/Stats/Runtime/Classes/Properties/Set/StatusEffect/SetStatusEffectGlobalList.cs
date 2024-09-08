using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the StatusEffect value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable]
    public class SetStatusEffectGlobalList : PropertyTypeSetStatusEffect
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueStatusEffect.TYPE_ID);

        public override void Set(StatusEffect value, Args args) => this.m_Variable.Set(value, args);
        public override StatusEffect Get(Args args) => this.m_Variable.Get(args) as StatusEffect;

        public static PropertySetStatusEffect Create => new PropertySetStatusEffect(
            new SetStatusEffectGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}