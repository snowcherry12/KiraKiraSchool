using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class StatusEffectOrAny
    {
        private enum Option
        {
            Any = 0,
            StatusEffect = 1
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private Option m_Option = Option.Any;

        [SerializeField]
        private PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool Any => this.m_Option == Option.Any;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public StatusEffectOrAny()
        { }

        public StatusEffectOrAny(StatusEffect statusEffect) : this()
        {
            this.m_Option = Option.StatusEffect;
            this.m_StatusEffect = new PropertyGetStatusEffect(
                new GetStatusEffectInstance(statusEffect)
            );
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(IdString statusEffectID, Args args)
        {
            if (this.Any) return true;
            StatusEffect statusEffect = this.m_StatusEffect.Get(args);
            
            if (statusEffect == null) return false;
            return statusEffect.ID.Hash == statusEffectID.Hash;
        }
    }
}
