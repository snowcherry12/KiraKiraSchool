using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Human IK")]
    [Category("Human IK")]
    
    [Description("Rotates a human bone chain and human hands in order to reach the desired direction")]
    [Image(typeof(IconAlignIK), ColorTheme.Type.Green)]
    
    [Serializable]
    public class BiomechanicsHumanIK : TBiomechanicsHuman
    {
        [SerializeField] private HumanRotationPitchIK m_BonesPitch = new HumanRotationPitchIK();
        [SerializeField] private HumanRotationYawIK m_BonesYaw = new HumanRotationYawIK();
        [SerializeField] private HumanRotationLean m_BonesLean = new HumanRotationLean();
        
        [SerializeField] private PropertyGetDecimal m_MaxPitch = GetDecimalDecimal.Create(160f);
        [SerializeField] private PropertyGetDecimal m_MaxYaw = GetDecimalDecimal.Create(120f);
        
        [SerializeField] private PropertyGetDecimal m_SwayWeight = GetDecimalDecimal.Create(0.75f);
        [SerializeField] private PropertyGetDecimal m_Sway = GetDecimalDecimal.Create(0.025f);
        
        [SerializeField] private EnablerLayerMask m_PullOnObstruction = new EnablerLayerMask(true);
        [SerializeField] private PropertyGetDecimal m_MinDistance = GetDecimalConstantPointOne.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override THumanRotations HumanBonesPitch => this.m_BonesPitch;
        public override THumanRotations HumanBonesYaw => this.m_BonesYaw;
        public override THumanRotations HumanBonesLean => this.m_BonesLean;

        public EnablerLayerMask PullOnObstruction => this.m_PullOnObstruction;
        
        // GETTER METHODS: ------------------------------------------------------------------------
        
        public override float GetMaxPitch(Args args)
        {
            return (float) this.m_MaxPitch.Get(args);
        }

        public override float GetMaxYaw(Args args)
        {
            return (float) this.m_MaxYaw.Get(args);
        }

        public float GetMinDistance(Args args)
        {
            return (float) this.m_MinDistance.Get(args);
        }

        public float GetSwayWeight(Args args)
        {
            return (float) this.m_SwayWeight.Get(args);
        }
        
        public float GetSway(Args args)
        {
            return (float) this.m_Sway.Get(args);
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