using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class FMODRepository : TRepository<FMODRepository>
    {        
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => "core.fmod";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private FMOD.Mixer m_FMODMixer = new ();

        // PROPERTIES: ----------------------------------------------------------------------------

        public FMOD.Mixer Mixer => this.m_FMODMixer;
    }
}