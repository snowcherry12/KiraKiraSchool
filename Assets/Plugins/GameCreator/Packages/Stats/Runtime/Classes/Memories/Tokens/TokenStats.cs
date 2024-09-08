using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class TokenStats : Token
    {
        [Serializable]
        private struct Entry
        {
            public int hash;
            public double value;
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField]
        private Entry[] m_Stats;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenStats(Traits traits) : base()
        {
            List<int> statsKeys = traits.RuntimeStats.StatsKeys;
            int statsKeysCount = statsKeys.Count;
            
            this.m_Stats = new Entry[statsKeysCount];
            for (int i = 0; i < statsKeysCount; ++i)
            {
                int hash = statsKeys[i];
                double value = traits.RuntimeStats.Get(hash).Base;
                
                this.m_Stats[i] = new Entry
                {
                    hash = hash,
                    value = value
                };
            }
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void OnRemember(Traits traits, Token token)
        {
            if (token is TokenStats tokenStats)
            {
                foreach (Entry entry in tokenStats.m_Stats)
                {
                    RuntimeStatData runtimeStatData = traits.RuntimeStats.Get(entry.hash); 
                    if (runtimeStatData != null) runtimeStatData.Base = entry.value;
                }
            }
        }
    }
}