using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Icon(EditorPaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoShooterWeapon.png")]
    
    [Serializable]
    public class ShooterWeapon : TWeapon, IStageGizmos, ISerializationCallbackReceiver
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInitializeOnLoad()
        {
            LastShotData = default;
        }
        
        private const float TRANSITION = 0.25f;

        public static ShotData LastShotData { get; private set; }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private StateData m_State = new StateData(StateData.StateType.State);
        [SerializeField] private PropertyGetInteger m_Layer = GetDecimalInteger.Create(7);
        
        [SerializeField] private Magazine m_Magazine = new Magazine();
        [SerializeField] private Muzzle m_Muzzle = new Muzzle();
        [SerializeField] private Fire m_Fire = new Fire();
        [SerializeField] private Projectile m_Projectile = new Projectile();
        [SerializeField] private Accuracy m_Accuracy = new Accuracy();
        [SerializeField] private Recoil m_Recoil = new Recoil();
        [SerializeField] private Shell m_Shell = new Shell();
        [SerializeField] private Jam m_Jam = new Jam();
        
        [SerializeField] private SightList m_Sights = new SightList();
        [SerializeField] private ReloadList m_Reloads = new ReloadList();

        [SerializeField] private RunConditionsList m_CanShoot = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_OnShoot = new RunInstructionsList();
        
        [SerializeField] private RunInstructionsList m_OnStartReload = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnFinishReload = new RunInstructionsList();
        
        [SerializeField] private RunConditionsList m_CanHit = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_OnHit = new RunInstructionsList();

        [SerializeField] private Animations m_Animations = new Animations();
        [SerializeField] protected AnimatorOverrideController m_Controller;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override IShield Shield => null;
        
        public override Texture EditorIcon => new IconPistol(
            ColorTheme.Get(ColorTheme.Type.Red)
        ).Texture;
        
        public Magazine Magazine => this.m_Magazine;
        public Muzzle Muzzle => this.m_Muzzle;
        public Fire Fire => this.m_Fire;
        public Projectile Projectile => this.m_Projectile;
        public Accuracy Accuracy => this.m_Accuracy;
        public Recoil Recoil => this.m_Recoil;
        public Shell Shell => this.m_Shell;
        public Jam Jam => this.m_Jam;
        
        public SightList Sights => this.m_Sights;
        
        [field: SerializeField] public string EditorModelPath { get; set; }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Scope GetScope(Character character)
        {
            GameObject instance = character.Combat.GetProp(this);
            if (instance == null) return default;
            
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            WeaponData weaponData = stance.Get(this);
            
            SightItem sight = weaponData != null ? this.Sights.Get(weaponData.SightId) : null;
            if (sight == null || sight.ScopeThrough == false) return default;

            return new Scope(
                instance.transform,
                sight.ScopePosition,
                sight.ScopeRotation,
                sight.ScopeDistance
            );
        }
        
        public Reload GetReload(Character character, bool isFull)
        {
            return this.m_Reloads.Pick(character, isFull);
        }
        
        public override async Task RunOnEquip(Character character, Args args)
        {
            character.Combat.RequestStance<ShooterStance>();
            
            if (args.Target != null)
            {
                Animator animator = args.Target.Require<Animator>();
                animator.runtimeAnimatorController = this.m_Controller;
            }
            
            if (this.m_State.IsValid(character))
            {
                ConfigState configuration = new ConfigState(0f, 1f, 1f, TRANSITION, 0f);
            
                _ = character.States.SetState(
                    this.m_State, (int) this.m_Layer.Get(args), 
                    BlendMode.Blend, configuration
                );
            }
            
            await base.RunOnEquip(character, args);
        }

        public override async Task RunOnUnequip(Character character, Args args)
        {
            character.Combat.RequestStance<ShooterStance>();
            
            if (this.m_State.IsValid(character))
            {
                int layer = (int) this.m_Layer.Get(args);
                character.States.Stop(layer, 0f, TRANSITION);
            }
            
            await base.RunOnUnequip(character, args);
        }

        public override TMunitionValue CreateMunition()
        {
            return new ShooterMunition();
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal bool CanShoot(ShotData data, Args args)
        {
            LastShotData = data;
            
            if (this.m_Magazine.EnoughAmmo(data, args) == false) return false;
            if (this.m_CanShoot.Check(args) == false) return false;
            
            SightItem sight = this.Sights.Get(data.SightId);
            return sight != null && sight.CanShoot(args);
        }
        
        internal void OnShoot(ShotData data, Args args)
        {
            LastShotData = data;
            _ = this.m_OnShoot.Run(args);

            if (data.Source.Combat.RequestMunition(data.Weapon) is ShooterMunition munition)
            {
                munition.InMagazine -= data.Cartridges;
            }

            data.Weapon.Accuracy.OnShoot(args);
            data.Weapon.Recoil.OnShoot(args);
        }
        
        internal bool CanHit(ShotData data, Args args)
        {
            LastShotData = data;
            return this.m_CanHit.Check(args);
        }
        
        internal void OnHit(ShotData data, Args args)
        {
            LastShotData = data;

            Trigger[] triggers = data.Target.GetComponents<Trigger>();
            foreach (Trigger trigger in triggers)
            {
                CommandArgs commandArgs = new CommandArgs(
                    EventShooterHit.COMMAND_HIT,
                    data.Source.gameObject
                );
                    
                trigger.OnReceiveCommand(commandArgs);
            }
            
            if (data.Weapon.Fire.ForceEnabled)
            {
                Rigidbody rigidbody = data.Target.Get<Rigidbody>();
                if (rigidbody != null)
                {
                    Vector3 force = data.ShootDirection.normalized * data.Weapon.Fire.Force;
                    rigidbody.AddForce(force, ForceMode.Impulse);
                }
            }
            
            Character target = data.Target.Get<Character>();
            if (target != null)
            {
                ReactionInput reactionInput = new ReactionInput(
                    data.ShootDirection.normalized,
                    (float) data.Weapon.Fire.Power(args)
                );
                
                _ = target.Combat.GetHitReaction(
                    reactionInput,
                    args,
                    data.Weapon.HitReaction
                );
            }
            
            _ = this.m_OnHit.Run(args);
        }

        internal async Task RunOnEnterReload(Character character, float speed, Args args)
        {
            _ = this.m_OnStartReload.Run(args);
            await this.m_Reloads.EnterState(character, speed, args);
        }
        
        internal async Task RunOnExitReload(Character character, float speed, CancelReloadSequence cancel, Args args)
        {
            _ = this.m_OnFinishReload.Run(args);
            await this.m_Reloads.ExitState(character, speed, cancel, args);
        }

        // STAGE GIZMOS: --------------------------------------------------------------------------

        void IStageGizmos.StageGizmos(StagingGizmos stagingGizmos)
        {
            this.m_Sights.StageGizmos(stagingGizmos);
            
            this.m_Magazine.StageGizmos(stagingGizmos);
            this.m_Muzzle.StageGizmos(stagingGizmos);
            this.m_Fire.StageGizmos(stagingGizmos);
            this.m_Projectile.StageGizmos(stagingGizmos);
            this.m_Recoil.StageGizmos(stagingGizmos);
            this.m_Shell.StageGizmos(stagingGizmos);
            this.m_Jam.StageGizmos(stagingGizmos);
        }
        
        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            
            if (AssemblyUtils.IsReloading) return;
            
            if (this.m_Controller == null) return;
            
            this.m_Controller["Weapon@Charge0"] = this.m_Animations.ChargeProgressMin;
            this.m_Controller["Weapon@Charge1"] = this.m_Animations.ChargeProgressMax;
            this.m_Controller["Weapon@Empty"] = this.m_Animations.Empty;
            this.m_Controller["Weapon@Idle"] = this.m_Animations.Idle;
            this.m_Controller["Weapon@JamEnter"] = this.m_Animations.JamEnter;
            this.m_Controller["Weapon@JamExit"] = this.m_Animations.JamExit;
            this.m_Controller["Weapon@JamIdle"] = this.m_Animations.Jammed;
            this.m_Controller["Weapon@Shoot"] = this.m_Animations.Shoot;
            this.m_Controller["Weapon@ReloadQuick"] = this.m_Animations.ReloadQuick;
            this.m_Controller["Weapon@ReloadDry"] = this.m_Animations.ReloadDry;
            
            #endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}