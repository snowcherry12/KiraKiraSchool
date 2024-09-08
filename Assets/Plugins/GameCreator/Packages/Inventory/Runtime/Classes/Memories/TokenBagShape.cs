using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public struct TokenBagShape
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private int m_MaxWidth;
        [SerializeField] private int m_MaxHeight;

        // PROPERTIES: ----------------------------------------------------------------------------

        public int MaxWidth
        {
            get => this.m_MaxWidth;
            internal set => this.m_MaxWidth = value;
        }
        
        public int MaxHeight
        {
            get => this.m_MaxHeight;
            internal set => this.m_MaxHeight = value;
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenBagShape(IBagShape bagShape)
        {
            if (bagShape == null)
            {
                this.m_MaxWidth = -1;
                this.m_MaxHeight = -1;

                return;
            }
            
            this.m_MaxWidth = bagShape.MaxWidth;
            this.m_MaxHeight = bagShape.MaxHeight;
        }
    }
}