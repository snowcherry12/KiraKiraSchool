using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public struct TokenBagItems
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private Bucket[] m_RuntimeItems;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Bucket[] RuntimeItems
        {
            get => this.m_RuntimeItems;
            internal set => this.m_RuntimeItems = value;
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenBagItems(IBagContent bagContent)
        {
            if (bagContent == null)
            {
                this.m_RuntimeItems = Array.Empty<Bucket>();
                return;
            }

            List<Bucket> result = bagContent.RuntimeItemsClone;
            this.m_RuntimeItems = result.ToArray();
        }
    }
}