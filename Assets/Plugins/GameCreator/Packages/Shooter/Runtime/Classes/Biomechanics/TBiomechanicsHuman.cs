using System;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public abstract class TBiomechanicsHuman : TBiomechanics
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private HumanRecoil m_Recoil = new HumanRecoil();
        [SerializeField] private HumanFreeHand m_FreeHand = new HumanFreeHand();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override HumanRecoil HumanRecoil => this.m_Recoil;
        public override HumanFreeHand HumanFreeHand => this.m_FreeHand;
    }
}