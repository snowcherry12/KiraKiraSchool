using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Bucket
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private RuntimeItem[] m_RuntimeItems;

        // PROPERTIES: ----------------------------------------------------------------------------
            
        public RuntimeItem[] RuntimeItems => this.m_RuntimeItems;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Bucket()
        {
            this.m_RuntimeItems = Array.Empty<RuntimeItem>();
        }
        
        public Bucket(RuntimeItem[] runtimeItems)
        {
            this.m_RuntimeItems = runtimeItems;
        }
    }
}