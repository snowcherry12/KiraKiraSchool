using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effect Description")]
    [Category("Stats/Status Effect Description")]

    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Returns the description text of a Status Effect")]
    
    [Serializable]
    public class GetStringStatusEffectDescription : PropertyTypeGetString
    {
        [SerializeField]
        protected PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();

        public override string Get(Args args)
        {
            StatusEffect statusEffect = this.m_StatusEffect.Get(args);
            return statusEffect != null
                ? statusEffect.GetDescription(args)
                : string.Empty;
        }

        public override string String => this.m_StatusEffect.ToString();
    }
}