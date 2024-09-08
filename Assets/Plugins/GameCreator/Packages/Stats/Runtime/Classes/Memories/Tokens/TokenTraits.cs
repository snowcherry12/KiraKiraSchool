using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class TokenTraits : Token
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private TokenStats m_Stats;
        [SerializeField] private TokenAttributes m_Attributes;
        [SerializeField] private TokenStatusEffects m_StatusEffects;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenTraits(Traits traits) : base()
        {
            this.m_Stats = new TokenStats(traits);
            this.m_Attributes = new TokenAttributes(traits);
            this.m_StatusEffects = new TokenStatusEffects(traits);
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void OnRemember(Traits traits, Token token)
        {
            if (token is TokenTraits tokenTraits)
            {
                TokenStats.OnRemember(traits, tokenTraits.m_Stats);
                TokenAttributes.OnRemember(traits, tokenTraits.m_Attributes);
                TokenStatusEffects.OnRemember(traits, tokenTraits.m_StatusEffects);
            }
        }
    }
}