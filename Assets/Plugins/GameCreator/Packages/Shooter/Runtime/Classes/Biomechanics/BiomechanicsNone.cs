using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Shooter
{
    [Title("None")]
    [Category("None")]
    
    [Description("Does not use any post-processing to align the weapon")]
    [Image(typeof(IconEmpty), ColorTheme.Type.TextLight)]
    
    [Serializable]
    public class BiomechanicsNone : TBiomechanics
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override THumanRotations HumanBonesPitch => null;
        public override THumanRotations HumanBonesYaw => null;
        public override THumanRotations HumanBonesLean => null;

        public override HumanRecoil HumanRecoil => null;
        public override HumanFreeHand HumanFreeHand => null;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override float GetMaxPitch(Args args) => 0f;
        public override float GetMaxYaw(Args args) => 0f;

        public override void Enter(Character character, ShooterWeapon weapon, float enterDuration)
        { }

        public override void Exit(Character character, ShooterWeapon weapon, float exitDuration)
        { }
    }
}