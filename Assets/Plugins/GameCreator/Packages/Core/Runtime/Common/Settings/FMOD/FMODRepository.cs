using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class FMODRepository : TRepository<FMODRepository>
    {
        public const string REPOSITORY_ID = "core.fmod";
        
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => REPOSITORY_ID;

        // MEMBERS: -------------------------------------------------------------------------------


        // PROPERTIES: ----------------------------------------------------------------------------

        // public WelcomeData WelcomeData
        // {
        //     get => this.m_WelcomeData;
        //     set => this.m_WelcomeData = value;
        // }
    }
}