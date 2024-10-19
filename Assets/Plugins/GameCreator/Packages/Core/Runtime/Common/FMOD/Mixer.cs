using System;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common.FMOD
{
    [Serializable]
    public class Mixer
    {
        // // MEMBERS: -------------------------------------------------------------------------------
        // [NonSerialized] private Dictionary<IdString, SoundEffectsAsset> m_MapSoundEffects;

        // // EXPOSED MEMBERS: -----------------------------------------------------------------------
        // [SerializeField] private SoundEffectsAsset[] m_SoundEffects = Array.Empty<SoundEffectsAsset>();

        // // PROPERTIES: ----------------------------------------------------------------------------
        // public SoundEffectsAsset[] SoundEffects => this.m_SoundEffects;

        // // PUBLIC METHODS: ------------------------------------------------------------------------
        // public SoundEffectsAsset GetSoundEffectsAsset(IdString itemID)
        // {
        //     // this.RequireInitialize();
        //     return this.m_MapSoundEffects.TryGetValue(itemID, out SoundEffectsAsset sfx)
        //         ? sfx
        //         : null;
        // }
    }
}