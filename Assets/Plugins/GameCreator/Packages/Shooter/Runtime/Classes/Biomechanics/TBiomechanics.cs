using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Biomechanics")]
    
    [Serializable]
    public abstract class TBiomechanics : TPolymorphicItem<TBiomechanics>, IBiomechanics
    {
        [SerializeField] private PropertyGetPosition m_Optics = GetPositionCharacterBone.Create(
            GetGameObjectSelf.Create(),
            new Bone(HumanBodyBones.Head)
        );
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public abstract THumanRotations HumanBonesPitch { get; }
        public abstract THumanRotations HumanBonesYaw { get; }
        public abstract THumanRotations HumanBonesLean { get; }
        
        public abstract HumanRecoil HumanRecoil { get; }
        public abstract HumanFreeHand HumanFreeHand { get; }
        
        // GETTER METHODS: ------------------------------------------------------------------------
        
        public abstract float GetMaxPitch(Args args);
        public abstract float GetMaxYaw(Args args);

        public Vector3 GetOpticsPoint(Args args) => this.m_Optics.Get(args);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public abstract void Enter(Character character, ShooterWeapon weapon, float enterDuration);

        public abstract void Exit(Character character, ShooterWeapon weapon, float exitDuration);
    }
}