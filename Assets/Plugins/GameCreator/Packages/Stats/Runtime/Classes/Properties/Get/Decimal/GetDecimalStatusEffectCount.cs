using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Status Effect Count")]
    [Category("Stats/Status Effect Count")]

    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("The amount stacked of an active Status Effect on an object's Traits component")]

    [Serializable]
    public class GetDecimalStatusEffectCount : PropertyTypeGetDecimal
    {
        [SerializeField]
        private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();

        [SerializeField]
        protected PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();

        public override double Get(Args args)
        {
            StatusEffect statusEffect = this.m_StatusEffect.Get(args);
            if (statusEffect == null) return 0f;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return 0f;
            
            return traits.RuntimeStatusEffects.GetActiveStackCount(statusEffect.ID);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalStatusEffectCount()
        );

        public override string String => $"{this.m_Traits}[{this.m_StatusEffect}].Count";
    }
}