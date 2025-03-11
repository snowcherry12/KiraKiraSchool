using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class SightCrosshair
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_UseCrosshair = GetBoolCharacterIsPlayer.Create;
        [SerializeField] private Crosshair m_Skin;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Crosshair Skin => this.m_Skin;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool UseCrosshair(Args args) => this.m_UseCrosshair.Get(args);
    }
}