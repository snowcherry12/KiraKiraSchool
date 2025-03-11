using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Human FK")]
    [Category("Human FK")]
    
    [Description("Rotates a human bone chain in order to reach the desired direction")]
    [Image(typeof(IconAlignFK), ColorTheme.Type.Green)]
    
    [Serializable]
    public class BiomechanicsHumanFK : TBiomechanicsHuman
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private HumanRotationPitchFK m_BonesPitch = new HumanRotationPitchFK();
        [SerializeField] private HumanRotationYawFK m_BonesYaw = new HumanRotationYawFK();
        [SerializeField] private HumanRotationLean m_BonesLean = new HumanRotationLean();
        
        [SerializeField] private PropertyGetDecimal m_MaxPitch = GetDecimalDecimal.Create(160f);
        [SerializeField] private PropertyGetDecimal m_MaxYaw = GetDecimalDecimal.Create(120f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override THumanRotations HumanBonesPitch => this.m_BonesPitch;
        public override THumanRotations HumanBonesYaw => this.m_BonesYaw;
        public override THumanRotations HumanBonesLean => this.m_BonesLean;

        // GETTER METHODS: ------------------------------------------------------------------------
        
        public override float GetMaxPitch(Args args)
        {
            return (float) this.m_MaxPitch.Get(args);
        }

        public override float GetMaxYaw(Args args)
        {
            return (float) this.m_MaxYaw.Get(args);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void Enter(Character character, ShooterWeapon weapon, float enterDuration)
        {
            character.IK.RequireRig<RigShooterHuman>()?.OnEnter(weapon, this, enterDuration);
        }

        public override void Exit(Character character, ShooterWeapon weapon, float exitDuration)
        {
            character.IK.GetRig<RigShooterHuman>()?.OnExit(weapon, this, exitDuration);
        }
    }
}