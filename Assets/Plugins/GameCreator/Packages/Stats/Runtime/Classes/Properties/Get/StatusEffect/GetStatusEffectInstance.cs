using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effect")]
    [Category("Status Effect")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("A direct reference to the Status Effect value")]

    [Serializable] [HideLabelsInEditor]
    public class GetStatusEffectInstance : PropertyTypeGetStatusEffect
    {
        [SerializeField] protected StatusEffect m_StatusEffect;

        public GetStatusEffectInstance()
        { }

        public GetStatusEffectInstance(StatusEffect statusEffect)
        {
            this.m_StatusEffect = statusEffect;
        }

        public override StatusEffect Get(Args args) => this.m_StatusEffect;
        public override StatusEffect Get(GameObject gameObject) => this.m_StatusEffect;

        public override string String => this.m_StatusEffect != null
            ? $"{this.m_StatusEffect.name}"
            : "(none)";
    }
}