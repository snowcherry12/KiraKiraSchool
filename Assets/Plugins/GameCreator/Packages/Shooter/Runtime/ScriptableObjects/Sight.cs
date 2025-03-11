using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [CreateAssetMenu(
        fileName = "Sight", 
        menuName = "Game Creator/Shooter/Sight",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoSight.png")]
    
    [Serializable]
    public class Sight : ScriptableObject
    {
        internal const float TRANSITION = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_CanShoot = GetBoolTrue.Create;
        
        [SerializeField]
        private PropertyGetInteger m_Priority = new PropertyGetInteger(
            new GetDecimalConstantOne()
        );
        
        [SerializeField] private EnablerFloat m_SmoothTime = new EnablerFloat(true, 0.1f);
        
        [SerializeField] private StateData m_State = new StateData(StateData.StateType.State);
        [SerializeField] private PropertyGetInteger m_Layer = GetDecimalInteger.Create(8);
        
        [SerializeField] private RunInstructionsList m_OnEnter = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnExit = new RunInstructionsList();
        
        [SerializeField] private Aim m_Aim = new Aim();
        [SerializeField] private Biomechanics m_Biomechanics = new Biomechanics();
        
        [SerializeField] private SightTrajectory m_Trajectory = new SightTrajectory();
        [SerializeField] private SightCrosshair m_Crosshair = new SightCrosshair();
        [SerializeField] private SightLaser m_Laser = new SightLaser();
        
        [SerializeField] private bool m_ReloadingUsesFK;
        [SerializeField] private bool m_ReloadingUsesIK;
        [SerializeField] private bool m_ShootingUsesFK;
        [SerializeField] private bool m_ShootingUsesIK;
        [SerializeField] private bool m_FixingUsesFK;
        [SerializeField] private bool m_FixingUsesIK;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool ReloadingUsesFK => this.m_ReloadingUsesFK;
        public bool ReloadingUsesIK => this.m_ReloadingUsesIK;
        
        public bool ShootingUsesFK => this.m_ShootingUsesFK;
        public bool ShootingUsesIK => this.m_ShootingUsesIK;
        
        public bool FixingUsesFK => this.m_FixingUsesFK;
        public bool FixingUsesIK => this.m_FixingUsesIK;
        
        public float SmoothTime => this.m_SmoothTime.IsEnabled
            ? this.m_SmoothTime.Value
            : 0f;
        
        public Aim Aim => this.m_Aim;
        public Biomechanics Biomechanics => this.m_Biomechanics;
        public SightTrajectory Trajectory => this.m_Trajectory;
        public SightCrosshair Crosshair => this.m_Crosshair;
        public SightLaser Laser => this.m_Laser;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool CanShoot(Args args)
        {
            return this.m_CanShoot.Get(args);
        }

        public int GetPriority(Args args)
        {
            return (int) this.m_Priority.Get(args);
        }

        public MuzzleData GetMuzzle(Args args, ShooterWeapon weapon)
        {
            Vector3 muzzlePosition = weapon.Muzzle.GetPosition(args);
            Quaternion muzzleRotation = weapon.Muzzle.GetRotation(args);
            
            Vector3 muzzleDirection = muzzleRotation * Vector3.forward;
            return new MuzzleData(muzzlePosition, muzzleDirection);
        }

        public Vector3 GetSpreadDirection(Args args, ShooterWeapon weapon)
        {
            Vector3 direction = this.GetMuzzle(args, weapon).Direction;
            
            Vector3 axisPitch = Vector3.Cross(direction, Vector3.up);
            Vector3 axisYaw = Vector3.up;
            
            Vector2 spread = weapon.Accuracy.CalculateSpread(args);
            
            Quaternion spreadPitch = Quaternion.AngleAxis(spread.y, axisPitch);
            direction = spreadPitch * direction;
            
            Quaternion spreadYaw = Quaternion.AngleAxis(spread.x, axisYaw);
            direction = spreadYaw * direction;

            return direction;
        }

        public void Enter(Character character, ShooterWeapon weapon)
        {
            this.m_Aim.Value.Enter(character);

            float enterDuration = 0f;
            
            if (this.m_State.IsValid(character))
            {
                ConfigState configuration = new ConfigState(0f, 1f, 1f, TRANSITION, 0f);
            
                _ = character.States.SetState(
                    this.m_State, (int) this.m_Layer.Get(character.Args), 
                    BlendMode.Blend, configuration
                );

                if (this.m_State.Type == StateData.StateType.State)
                {
                    enterDuration = this.m_State.State != null && this.m_State.State.HasEntryClip
                        ? this.m_State.State.EntryClip.length
                        : 0f;
                }
            }
            
            this.m_Aim.Enter(character);
            this.m_Biomechanics.Enter(character, weapon, enterDuration);
            
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            WeaponData weaponData = stance.Get(weapon);
            
            _ = this.m_OnEnter.Run(weaponData.WeaponArgs);
        }
        
        public void Exit(Character character, ShooterWeapon weapon)
        {
            this.m_Aim.Value.Exit(character);
            
            if (this.m_State.IsValid(character))
            {
                int layer = (int) this.m_Layer.Get(character.Args);
                character.States.Stop(layer, 0f, TRANSITION);
            }
            
            this.m_Aim.Exit(character);
            this.m_Biomechanics.Exit(character, weapon, TRANSITION);
            
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            WeaponData weaponData = stance.Get(weapon);
            
            _ = this.m_OnExit.Run(weaponData.WeaponArgs);
        }
    }
}