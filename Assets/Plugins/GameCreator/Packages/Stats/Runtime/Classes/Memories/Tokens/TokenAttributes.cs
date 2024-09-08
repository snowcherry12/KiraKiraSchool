using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class TokenAttributes : Token
    {
        [Serializable]
        public struct Entry
        {
            public int hash;
            public double value;
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField]
        private Entry[] m_Attributes;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenAttributes(Traits traits) : base()
        {
            List<int> attributesKeys = traits.RuntimeAttributes.AttributesKeys;
            int attributesKeysCount = attributesKeys.Count;
            
            this.m_Attributes = new Entry[attributesKeysCount];
            for (int i = 0; i < attributesKeysCount; ++i)
            {
                int hash = attributesKeys[i];
                double value = traits.RuntimeAttributes.Get(hash).Value;
                
                this.m_Attributes[i] = new Entry
                {
                    hash = hash,
                    value = value
                };
            }
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void OnRemember(Traits traits, Token token)
        {
            if (token is TokenAttributes tokenAttributes)
            {
                foreach (Entry entry in tokenAttributes.m_Attributes)
                {
                    RuntimeAttributeData runtimeAttrData = traits.RuntimeAttributes.Get(entry.hash); 
                    if (runtimeAttrData != null) runtimeAttrData.Value = entry.value;
                }
            }
        }
    }
}