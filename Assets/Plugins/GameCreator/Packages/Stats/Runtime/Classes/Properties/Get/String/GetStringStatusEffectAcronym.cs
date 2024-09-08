using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effect Acronym")]
    [Category("Stats/Status Effect Acronym")]

    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Returns the acronym of a Status Effect")]
    
    [Serializable]
    public class GetStringStatusEffectAcronym : PropertyTypeGetString
    {
        [SerializeField]
        protected PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();

        public override string Get(Args args)
        {
            StatusEffect statusEffect = this.m_StatusEffect.Get(args);
            return statusEffect != null
                ? statusEffect.GetAcronym(args)
                : string.Empty;
        }

        public override string String => this.m_StatusEffect.ToString();
    }
}