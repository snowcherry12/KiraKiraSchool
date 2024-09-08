using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effect Name")]
    [Category("Stats/Status Effect Name")]

    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Returns the name of a Status Effect")]
    
    [Serializable]
    public class GetStringStatusEffectName : PropertyTypeGetString
    {
        [SerializeField]
        protected PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();

        public override string Get(Args args)
        {
            StatusEffect statusEffect = this.m_StatusEffect.Get(args);
            return statusEffect != null
                ? statusEffect.GetName(args)
                : string.Empty;
        }

        public override string String => this.m_StatusEffect.ToString();
    }
}