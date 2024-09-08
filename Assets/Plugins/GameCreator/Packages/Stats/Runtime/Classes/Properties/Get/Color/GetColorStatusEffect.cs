using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effect Color")]
    [Category("Stats/Status Effect Color")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Returns the Color value of a Status Effect")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorStatusEffect : PropertyTypeGetColor
    {
        [SerializeField]
        protected PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();

        public override Color Get(Args args)
        {
            StatusEffect statusEffect = this.m_StatusEffect.Get(args);
            return statusEffect != null
                ? statusEffect.Color
                : Color.black;
        }

        public override string String => this.m_StatusEffect.ToString();
    }
}