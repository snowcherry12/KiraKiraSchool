using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class TokenStatusEffects : Token
    {
        [Serializable]
        public struct Entry
        {
            public int hash;
            public float[] timeElapsed;
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField]
        private Entry[] m_StatusEffects;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenStatusEffects(Traits traits) : base()
        {
            List<IdString> statusEffectsIDs = traits.RuntimeStatusEffects.GetActiveList();
            int statusEffectsIDsCount = statusEffectsIDs.Count;
            
            this.m_StatusEffects = new Entry[statusEffectsIDsCount];
            for (int i = 0; i < statusEffectsIDsCount; ++i)
            {
                IdString statusEffectID = statusEffectsIDs[i];

                int stackCount = traits.RuntimeStatusEffects.GetActiveStackCount(statusEffectID);
                float[] timeElapsed = new float[stackCount];

                for (int j = 0; j < stackCount; ++j)
                {
                    RuntimeStatusEffectValue statusEffectValue = traits
                        .RuntimeStatusEffects
                        .GetActiveAt(statusEffectID, j);

                    timeElapsed[j] = statusEffectValue.TimeElapsed;
                }

                this.m_StatusEffects[i] = new Entry
                {
                    hash = statusEffectID.Hash,
                    timeElapsed = timeElapsed
                };
            }
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void OnRemember(Traits traits, Token token)
        {
            if (token is TokenStatusEffects tokenStatusEffects)
            {
                StatusEffects settings = Settings.From<StatsRepository>().StatusEffects;
                foreach (Entry entry in tokenStatusEffects.m_StatusEffects)
                {
                    StatusEffect statusEffect = settings.Find(entry.hash);
                    if (statusEffect == null) continue;
                    if (!statusEffect.Save) continue;

                    foreach (float timeElapsed in entry.timeElapsed)
                    {
                        traits.RuntimeStatusEffects.Add(statusEffect, timeElapsed);
                    }
                }
            }
        }
    }
}