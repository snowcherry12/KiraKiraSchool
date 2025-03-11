using System;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public struct CrosshairData
    {
        [field: NonSerialized] public GameObject Instance { get; }
        [field: NonSerialized] public CrosshairUI[] Elements { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public CrosshairData(GameObject instance)
        {
            this.Instance = instance;
            this.Elements = this.Instance.GetComponentsInChildren<CrosshairUI>();
        }
    }
}