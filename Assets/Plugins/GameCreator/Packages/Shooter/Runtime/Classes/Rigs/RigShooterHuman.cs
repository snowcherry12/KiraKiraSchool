using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shooter Human")]
    [Category("Shooter Human")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Green)]
    [Description("Shooter system that uses humanoid FK/IK to aim towards the desired direction")]
    
    [Serializable]
    public class RigShooterHuman : TRigShooter
    {
        protected const int ORDER_BEFORE_CAMERA = ApplicationManager.EXECUTION_ORDER_DEFAULT;
        protected const int ORDER_AFTER_CAMERA = ApplicationManager.EXECUTION_ORDER_LAST_EARLIER;

        public const string RIG_NAME = "RigShooterHuman";
        
        private const float DECAY_USE_KINEMATICS = 0.15f;
        private const float DECAY_FK_RESTORE = 0.15f;
        private const float DECAY_FK_ROTATE = 0.15f;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private ShooterStance m_Stance;
        [NonSerialized] private ShooterWeapon m_Weapon;
        
        [NonSerialized] private Sight m_Sight;
        [NonSerialized] private TBiomechanics m_Biomechanics;
        
        [NonSerialized] private WeaponData m_WeaponData;
        [NonSerialized] private GameObject m_WeaponProp;
        
        [NonSerialized] private SpringFloat m_UseFK;
        [NonSerialized] private SpringFloat m_UseIK;
        
        [NonSerialized] private SpringFloat m_Pitch;
        [NonSerialized] private SpringFloat m_Yaw;
        [NonSerialized] private SpringFloat m_Lean;
        
        [NonSerialized] private SpringVector3 m_MainHandPosition;
        
        [NonSerialized] private HumanFK m_PitchFK;
        [NonSerialized] private HumanFK m_YawFK;
        [NonSerialized] private HumanFK m_LeanFK;

        [NonSerialized] private HumanIK m_MainL;
        [NonSerialized] private HumanIK m_MainR;
        [NonSerialized] private HumanIK m_FreeL;
        [NonSerialized] private HumanIK m_FreeR;
        [NonSerialized] private HumanIK m_RecoilL;
        [NonSerialized] private HumanIK m_RecoilR;
        
        [NonSerialized] private RecoilIK m_Recoil;
        
        [NonSerialized] private Transform m_HandL;
        [NonSerialized] private Transform m_HandR;
        
        [NonSerialized] private Transform m_MainHand;
        [NonSerialized] private Transform m_FreeHand;
        
        [NonSerialized] private bool m_HasTargetPoint;
        [NonSerialized] private Vector3 m_TargetPoint;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Shooter Human";
        public override string Name => RIG_NAME;

        public override bool RequiresHuman => true;
        public override bool DisableOnBusy => true;

        [field: NonSerialized] protected float EnterDuration { get; private set; }
        [field: NonSerialized] protected float ExitDuration { get; private set; }

        [field: NonSerialized] public float LeanAmount { get; set; }
        [field: NonSerialized] public float LeanDecay { get; set; }

        // INITIALIZE METHODS: --------------------------------------------------------------------

        protected sealed override void DoEnable(Character character)
        {
            this.m_Stance = character.Combat.RequestStance<ShooterStance>();
            
            this.m_UseFK = new SpringFloat(0f, DECAY_USE_KINEMATICS);
            this.m_UseIK = new SpringFloat(0f, DECAY_USE_KINEMATICS);
            
            this.m_Pitch = new SpringFloat(0f);
            this.m_Yaw = new SpringFloat(0f);
            this.m_Lean = new SpringFloat(0f);
            
            this.m_MainHandPosition = new SpringVector3(Vector3.zero);
            
            this.m_HandL = character.Animim.Animator.GetBoneTransform(HumanBodyBones.LeftHand);
            this.m_HandR = character.Animim.Animator.GetBoneTransform(HumanBodyBones.RightHand);
            
            this.m_PitchFK = new HumanFK();
            this.m_YawFK = new HumanFK();
            this.m_LeanFK = new HumanFK();
            
            this.m_MainL = new HumanIK(character.Animim.Mannequin, this.m_HandL);
            this.m_MainR = new HumanIK(character.Animim.Mannequin, this.m_HandR);
            this.m_FreeL = new HumanIK(character.Animim.Mannequin, this.m_HandL);
            this.m_FreeR = new HumanIK(character.Animim.Mannequin, this.m_HandR);
            this.m_RecoilL = new HumanIK(character.Animim.Mannequin, this.m_HandL);
            this.m_RecoilR = new HumanIK(character.Animim.Mannequin, this.m_HandR);
            
            this.m_Recoil = new RecoilIK();
            
            base.DoEnable(character);
            
            UpdateManager.SubscribeLateUpdate(this.OnLateUpdateBeforeCamera, ORDER_BEFORE_CAMERA);
            UpdateManager.SubscribeLateUpdate(this.OnLateUpdateAfterCamera, ORDER_AFTER_CAMERA);
        }

        protected sealed override void DoDisable(Character character)
        {
            if (ApplicationManager.IsExiting) return;
            
            base.DoDisable(character);
            
            UpdateManager.UnsubscribeLateUpdate(this.OnLateUpdateBeforeCamera, ORDER_BEFORE_CAMERA);
            UpdateManager.UnsubscribeLateUpdate(this.OnLateUpdateAfterCamera, ORDER_AFTER_CAMERA);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OnEnter(ShooterWeapon weapon, TBiomechanics biomechanics, float enterDuration)
        {
            this.EnterDuration = enterDuration;
        }

        public void OnExit(ShooterWeapon weapon, TBiomechanics biomechanics, float exitDuration)
        {
            this.ExitDuration = exitDuration;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnLateUpdateBeforeCamera()
        {
            this.m_Weapon = null;
            this.m_Sight = null;
            
            int maxPriority = -1;
            
            foreach (Weapon combatWeapon in this.Character.Combat.Weapons)
            {
                ShooterWeapon shooterWeapon = combatWeapon.Asset as ShooterWeapon;
                if (shooterWeapon == null) continue;
                
                WeaponData weaponData = this.m_Stance.Get(shooterWeapon);
            
                Sight shooterSight = shooterWeapon.Sights.Get(weaponData.SightId)?.Sight;
                if (shooterSight == null) continue;
                
                int priority = shooterSight.GetPriority(weaponData.WeaponArgs);
                if (priority > maxPriority)
                {
                    this.m_Weapon = shooterWeapon;
                    this.m_Sight = shooterSight;
                    
                    maxPriority = priority;
                }
            }
            
            this.m_UseFK.Update(
                this.GetTargetFK(),
                DECAY_USE_KINEMATICS,
                this.Character.Time.DeltaTime
            );
            
            this.m_UseIK.Update(
                this.GetTargetIK(),
                DECAY_USE_KINEMATICS,
                this.Character.Time.DeltaTime
            );
            
            this.m_WeaponData = this.m_Stance.Get(this.m_Weapon);
            this.m_WeaponProp = this.Character.Combat.GetProp(this.m_Weapon);

            this.m_Biomechanics = this.m_Sight != null ? this.m_Sight.Biomechanics.Value : null;
            
            this.m_MainHand = this.m_WeaponProp != null
                ? this.m_WeaponProp.transform.parent
                : null;

            this.m_FreeHand = this.m_Biomechanics?.HumanFreeHand?.UseFreeHand ?? false
                ? this.m_Biomechanics?.HumanFreeHand.GetBone(this.Animator)
                : null;
            
            float deltaTime = this.Character.Time.DeltaTime;
            this.m_Lean.Target = this.LeanAmount;
            this.m_Lean.Decay = this.LeanDecay;
            
            this.m_Lean.Update(
                this.m_Lean.Target,
                this.m_Lean.Decay,
                deltaTime
            );
            
            TBiomechanics biomechanics = this.m_Sight != null
                ? this.m_Sight.Biomechanics?.Value
                : null;
            
            this.m_LeanFK.Update(
                biomechanics?.HumanBonesLean,
                this.m_WeaponData?.WeaponArgs ?? this.Args,
                DECAY_FK_ROTATE,
                deltaTime
            );
            
            this.m_LeanFK.RotateBody(this.Animator, this.RotateLean, this.m_UseFK.Current);
        }

        private void OnLateUpdateAfterCamera()
        {
            this.UpdateFK();
            
            float deltaTime = this.Character.Time.DeltaTime;
            
            this.m_Pitch.Update(
                this.m_Pitch.Target,
                this.m_Pitch.Decay,
                deltaTime
            );
            
            this.m_Yaw.Update(
                this.m_Yaw.Target,
                this.m_Yaw.Decay,
                deltaTime
            );
            
            TBiomechanics biomechanics = this.m_Sight != null
                ? this.m_Sight.Biomechanics?.Value
                : null;
            
            this.m_PitchFK.Update(
                biomechanics?.HumanBonesPitch,
                this.m_WeaponData?.WeaponArgs ?? this.Args,
                DECAY_FK_ROTATE,
                deltaTime
            );
            
            this.m_YawFK.Update(
                biomechanics?.HumanBonesYaw,
                this.m_WeaponData?.WeaponArgs ?? this.Args,
                DECAY_FK_ROTATE,
                deltaTime
            );

            this.m_PitchFK.RotateBody(this.Animator, this.RotatePitch, this.m_UseFK.Current);
            this.m_YawFK.RotateBody(this.Animator, this.RotateYaw, this.m_UseFK.Current);
            
            TwoBoneData sideL = new TwoBoneData(this.m_HandL.parent.parent, this.m_HandL.parent, this.m_HandL);
            TwoBoneData sideR = new TwoBoneData(this.m_HandR.parent.parent, this.m_HandR.parent, this.m_HandR);
            
            this.UpdateMainIK(ref sideL, ref sideR);

            this.m_MainL.Update(this.m_Sight, sideL, deltaTime, this.m_UseIK.Current);
            this.m_MainR.Update(this.m_Sight, sideR, deltaTime, this.m_UseIK.Current);
            
            sideL = new TwoBoneData(this.m_HandL.parent.parent, this.m_HandL.parent, this.m_HandL);
            sideR = new TwoBoneData(this.m_HandR.parent.parent, this.m_HandR.parent, this.m_HandR);
            
            this.UpdateRecoilIK(ref sideL, ref sideR);
            
            this.m_RecoilL.Update(this.m_Sight, sideL, deltaTime, this.IsActive ? 1f : 0f);
            this.m_RecoilR.Update(this.m_Sight, sideR, deltaTime, this.IsActive ? 1f : 0f);
            
            sideL = new TwoBoneData(this.m_HandL.parent.parent, this.m_HandL.parent, this.m_HandL);
            sideR = new TwoBoneData(this.m_HandR.parent.parent, this.m_HandR.parent, this.m_HandR);
            
            this.UpdateFreeIK(ref sideL, ref sideR);
            
            float freeHandDecay = 1f;
            if (this.m_Stance.Shooting.IsShootingAnimation)
            {
                if (!this.m_Sight.ShootingUsesIK) freeHandDecay = 0f;
            }
            
            this.m_FreeL.Update(this.m_Sight, sideL, deltaTime, this.m_UseIK.Current * freeHandDecay);
            this.m_FreeR.Update(this.m_Sight, sideR, deltaTime, this.m_UseIK.Current * freeHandDecay);
        }

        // FORWARD KINEMATICS: --------------------------------------------------------------------

        private void UpdateFK()
        {
            this.m_HasTargetPoint = false;

            if (this.m_Weapon == null || this.m_Sight == null || this.m_WeaponData == null)
            {
                this.m_Pitch.Target = 0f;
                this.m_Yaw.Target = 0f;
                
                this.m_Pitch.Decay = DECAY_FK_RESTORE;
                this.m_Yaw.Decay = DECAY_FK_RESTORE;
                
                return;
            }

            Args args = this.m_WeaponData.WeaponArgs;
            
            MuzzleData muzzle = this.m_Sight.GetMuzzle(args, this.m_Weapon);
            Vector3 opticsPoint = this.m_Sight.Biomechanics.Value.GetOpticsPoint(args);
            
            this.m_HasTargetPoint = true;
            this.m_TargetPoint = this.m_Sight.Aim.GetPoint(args);
            
            Vector3 opticsTargetDirection = this.m_TargetPoint - opticsPoint;
            
            this.CalculateRotation(
                muzzle.Direction,
                opticsTargetDirection,
                out float pitch,
                out float yaw
            );
            
            pitch = QuaternionUtils.Convert180(pitch);
            yaw = QuaternionUtils.Convert180(yaw);
            
            float maxPitch = this.m_Sight.Biomechanics.Value.GetMaxPitch(args) * 0.5f;
            float maxYaw = this.m_Sight.Biomechanics.Value.GetMaxYaw(args) * 0.5f;
            
            pitch = Math.Clamp(pitch, -maxPitch, maxPitch);
            yaw = Math.Clamp(yaw, -maxYaw, maxYaw);
            
            this.m_Pitch.Target = pitch;
            this.m_Yaw.Target = yaw;
            
            this.m_Pitch.Decay = this.m_Sight.SmoothTime;
            this.m_Yaw.Decay = this.m_Sight.SmoothTime;
        }
        
        // INVERSE KINEMATICS: --------------------------------------------------------------------

        private void UpdateMainIK(ref TwoBoneData sideL, ref TwoBoneData sideR)
        {
            if (this.m_WeaponProp == null || this.m_WeaponData == null || this.m_Sight == null)
            {
                return;
            }
            
            float deltaTime = this.Character.Time.DeltaTime;
            
            Scope scope = this.m_Weapon.GetScope(this.Character);
            bool useHandIK = this.m_MainHand == this.m_HandL || this.m_MainHand == this.m_HandR;
            
            if (scope.HasScope == false || useHandIK == false)
            {
                return;
            }
            
            Vector3 opticsPoint = this.m_Biomechanics.GetOpticsPoint(this.m_WeaponData.WeaponArgs);

            Quaternion rotationWeapon = 
                this.m_WeaponProp.transform.localRotation * 
                scope.LocalRotation *
                Quaternion.Euler(0f, 0f, -this.m_Lean.Current);
            
            Vector3 directionOpticsToTarget = this.m_HasTargetPoint
                ? this.m_TargetPoint - opticsPoint
                : Vector3.zero;
        
            Quaternion rotationOpticsToTarget = this.m_HasTargetPoint
                ? Quaternion.LookRotation(
                    directionOpticsToTarget.normalized,
                    Vector3.up
                ) : Quaternion.identity;
            
            Quaternion targetRotation = rotationOpticsToTarget * Quaternion.Inverse(rotationWeapon);
            
            float distance = scope.Distance;
            
            BiomechanicsHumanIK biomechanics = this.m_Biomechanics as BiomechanicsHumanIK;
            if (biomechanics?.PullOnObstruction.IsEnabled ?? false)
            {
                bool isHit = Physics.Raycast(
                    opticsPoint,
                    directionOpticsToTarget.normalized,
                    out RaycastHit hit,
                    directionOpticsToTarget.magnitude,
                    biomechanics.PullOnObstruction.Value,
                    QueryTriggerInteraction.Ignore
                );
                
                if (isHit)
                {
                    Vector3 muzzlePosition = this.m_Sight.GetMuzzle(
                        this.m_WeaponData.WeaponArgs,
                        this.m_Weapon
                    ).Position;
            
                    float targetDistance = Vector3.Distance(opticsPoint, hit.point);
                    float distanceMuzzleToScope = Vector3.Dot(
                        muzzlePosition - scope.WorldPosition,
                        directionOpticsToTarget.normalized
                    );
            
                    float pullDistance = targetDistance - distanceMuzzleToScope;
                    if (distance > pullDistance) distance = pullDistance;
                }
            }
        
            float minDistance = biomechanics?.GetMinDistance(this.m_WeaponData.WeaponArgs) ?? 0f;
            
            Vector3 distanceOffset = directionOpticsToTarget.normalized * Math.Max(minDistance, distance);
            Vector3 targetScopePosition = opticsPoint + distanceOffset;
            
            Vector3 parentOffset = this.Character.transform.InverseTransformDirection(
                this.m_MainHand.position - scope.WorldPosition
            );
            
            Vector3 axisForward = directionOpticsToTarget.normalized;
            Vector3 axisRight = Vector3.Cross(Vector3.up, axisForward).normalized;
            Vector3 axisUp = Vector3.Cross(axisForward, axisRight).normalized;
            
            parentOffset = parentOffset.x * axisRight +
                           parentOffset.y * axisUp +
                           parentOffset.z * axisForward;
            
            Vector3 mainHandPosition = RotatePivot(
                targetScopePosition,
                targetScopePosition + parentOffset,
                Quaternion.AngleAxis(scope.LocalRotation.eulerAngles.z, -axisForward)
            );
            
            float swayWeight = biomechanics?.GetSwayWeight(this.m_WeaponData.WeaponArgs) ?? 0f;
            Vector3 translationSway = this.Character.transform.position * swayWeight; 
            
            this.m_MainHandPosition.Update(
                mainHandPosition - translationSway,
                biomechanics?.GetSway(this.m_WeaponData.WeaponArgs) ?? 0f,
                deltaTime
            );
            
            TwoBoneData twoBoneData = new TwoBoneData(
                this.m_MainHand.parent.parent,
                this.m_MainHand.parent, 
                this.m_MainHand
            );

            twoBoneData = TwoBoneSolver.Run(
                twoBoneData,
                this.m_MainHandPosition.Current + translationSway,
                targetRotation,
                null,
                0f
            );
            
            if (this.m_MainHand == this.m_HandL) sideL = twoBoneData;
            if (this.m_MainHand == this.m_HandR) sideR = twoBoneData;
        }
        
        private void UpdateRecoilIK(ref TwoBoneData sideL, ref TwoBoneData sideR)
        {
            Args args = this.m_WeaponData?.WeaponArgs ?? this.Args;
            float deltaTime = this.Character.Time.DeltaTime;
            
            if (this.m_WeaponData != null && 
                this.m_Biomechanics?.HumanRecoil != null &&
                this.m_WeaponData.LastShotFrame == this.Character.Time.Frame - 1)
            {
                this.m_Recoil.OnShoot(
                    this.m_Biomechanics.HumanRecoil.GetPosition(args),
                    this.m_Biomechanics.HumanRecoil.GetRotation(args),
                    this.m_Biomechanics.HumanRecoil.GetKickback(args),
                    this.m_Biomechanics.HumanRecoil.GetImpactSpeed(args),
                    this.m_Biomechanics.HumanRecoil.GetRecoverSpeed(args)
                );
            }
            
            this.m_Recoil.Update(deltaTime);
            
            if (this.m_Biomechanics?.HumanRecoil == null) return;
            if (this.m_MainHand == null) return;
            if (this.m_Sight == null) return;
            
            MuzzleData muzzle = this.m_Sight.GetMuzzle(args, this.m_Weapon);

            Vector3 axisForward = muzzle.Direction.normalized;
            Vector3 axisRight = Vector3.Cross(Vector3.up, axisForward).normalized;
            Vector3 axisUpward = Vector3.Cross(axisForward, axisRight).normalized;
            
            Vector3 positionOffset = 
                axisForward * this.m_Recoil.Position.z +
                axisUpward * this.m_Recoil.Position.y +
                axisRight * this.m_Recoil.Position.x;
            
            Vector3 targetPosition = this.m_MainHand.position + positionOffset;
            
            TwoBoneData twoBoneData = new TwoBoneData(
                this.m_MainHand.parent.parent,
                this.m_MainHand.parent,
                this.m_MainHand
            );
            
            twoBoneData = TwoBoneSolver.Run(
                twoBoneData,
                targetPosition,
                this.m_MainHand.rotation,
                null,
                0f
            );

            Quaternion rotationOffset =
                Quaternion.AngleAxis(this.m_Recoil.Rotation.z, axisForward) *
                Quaternion.AngleAxis(this.m_Recoil.Rotation.y, axisUpward) *
                Quaternion.AngleAxis(this.m_Recoil.Rotation.x, axisRight);

            float root = this.m_Biomechanics.HumanRecoil.GetRatioUpperArm(args);
            float body = this.m_Biomechanics.HumanRecoil.GetRatioLowerArm(args);
            float head = this.m_Biomechanics.HumanRecoil.GetRatioHands(args);
            
            twoBoneData.HeadRotation = Quaternion.Lerp(Quaternion.identity, rotationOffset, head) * twoBoneData.HeadRotation;
            twoBoneData.BodyRotation = Quaternion.Lerp(Quaternion.identity, rotationOffset, body) * twoBoneData.BodyRotation;
            twoBoneData.RootRotation = Quaternion.Lerp(Quaternion.identity, rotationOffset, root) * twoBoneData.RootRotation;
            
            if (this.m_MainHand == this.m_HandL) sideL = twoBoneData;
            if (this.m_MainHand == this.m_HandR) sideR = twoBoneData;
        }

        private void UpdateFreeIK(ref TwoBoneData sideL, ref TwoBoneData sideR)
        {
            if (this.m_WeaponProp == null || this.m_WeaponData == null || this.m_Sight == null)
            {
                return;
            }
            
            if ((this.m_Biomechanics?.HumanFreeHand?.UseFreeHand ?? false) == false)
            {
                return;
            }

            if (this.m_FreeHand == null) return;
            
            Vector3 handlePosition = Vector3.zero;
            Quaternion handleRotation = Quaternion.identity;
            Transform handleTarget = null;
            
            this.m_Biomechanics.HumanFreeHand?.GetHandle(
                this.m_WeaponData.WeaponArgs,
                out handlePosition,
                out handleRotation,
                out handleTarget
            );
            
            if (handleTarget == null) return;
            
            TwoBoneData twoBoneData = new TwoBoneData(
                this.m_FreeHand.parent.parent,
                this.m_FreeHand.parent,
                this.m_FreeHand
            );
            
            Vector3 targetPosition = handleTarget.TransformPoint(handlePosition);
            Quaternion targetRotation = handleTarget.rotation * handleRotation;
            Transform pole = this.m_Biomechanics.HumanFreeHand?.GetPole(m_WeaponData.WeaponArgs);

            twoBoneData = TwoBoneSolver.Run(
                twoBoneData,
                targetPosition,
                targetRotation,
                pole,
                pole != null ? 1f : 0f
            );

            if (this.m_FreeHand == this.m_HandL) sideL = twoBoneData;
            if (this.m_FreeHand == this.m_HandR) sideR = twoBoneData;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void CalculateRotation(
            Vector3 muzzleDirection,
            Vector3 targetDirection,
            out float pitch,
            out float yaw)
        {
            muzzleDirection.Normalize();
            targetDirection.Normalize();
            
            if (muzzleDirection == targetDirection)
            {
                pitch = 0f;
                yaw = 0f;
                return;
            }
        
            float arctangentTargetDirection = Mathf.Atan2(targetDirection.x, targetDirection.z);
            float arctangentMuzzleDirection = Mathf.Atan2(muzzleDirection.x, muzzleDirection.z);
            
            yaw = (arctangentTargetDirection - arctangentMuzzleDirection) * Mathf.Rad2Deg;
            pitch = Vector3.SignedAngle(
                Quaternion.AngleAxis(yaw, Vector3.up) * muzzleDirection,
                targetDirection,
                Vector3.Cross(Vector3.up, targetDirection)
            );
        }
        
        private float GetTargetFK()
        {
            if (!this.IsActive) return 0f;
            if (this.m_Sight == null) return 0f;

            if (this.m_Stance.Reloading.IsReloading)
            {
                return this.m_Sight.ReloadingUsesFK ? 1f : 0f;
            }
            
            if (this.m_Stance.Shooting.IsShootingAnimation)
            {
                if (this.m_Sight.ShootingUsesFK) return 1f;
                return 1f - this.Character.Gestures.CurrentWeight;
            }
            
            if (this.m_Stance.Jamming.IsFixing)
            {
                if (this.m_Sight.FixingUsesFK) return 1f;
                return 1f - this.Character.Gestures.CurrentWeight;
            }

            return 1f;
        }
        
        private float GetTargetIK()
        {
            if (!this.IsActive) return 0f;
            if (this.m_Sight == null) return 0f;

            if (this.m_Stance.Reloading.IsReloading)
            {
                if (this.m_Sight.ReloadingUsesIK) return 1f;
                return 1f - this.Character.Gestures.CurrentWeight;
            }
            
            if (this.m_Stance.Shooting.IsShootingAnimation)
            {
                if (this.m_Sight.ShootingUsesIK) return 1f;
                return 1f - this.Character.Gestures.CurrentWeight;
            }
            
            if (this.m_Stance.Jamming.IsFixing)
            {
                if (this.m_Sight.FixingUsesIK) return 1f;
                return 1f - this.Character.Gestures.CurrentWeight;
            }

            return 1f;
        }

        // PRIVATE CALLBACKS: ---------------------------------------------------------------------

        private void RotatePitch(Transform bone, float ratio)
        {
            Vector3 axis = this.Character.transform.TransformDirection(Vector3.right);
            Vector3 boneAxis = bone.InverseTransformDirection(axis);
        
            bone.Rotate(boneAxis, this.m_Pitch.Current * ratio, Space.Self);
        }
        
        private void RotateYaw(Transform bone, float ratio)
        {
            Vector3 axis = this.Character.transform.TransformDirection(Vector3.up);
            Vector3 boneAxis = bone.InverseTransformDirection(axis);
        
            bone.Rotate(boneAxis, this.m_Yaw.Current * ratio, Space.Self);
        }
        
        private void RotateLean(Transform bone, float ratio)
        {
            Vector3 axis = this.Character.transform.TransformDirection(Vector3.forward);
            Vector3 boneAxis = bone.InverseTransformDirection(axis);
        
            bone.Rotate(boneAxis, this.m_Lean.Current * ratio, Space.Self);
        }
        
        private static Vector3 ConvertLocalToWorldDirection(
            Vector3 localDirection,
            Vector3 forwardDirection,
            Vector3 upDirection)
        {
            forwardDirection = forwardDirection.normalized;
            upDirection = upDirection.normalized;
            
            Vector3 rightDirection = Vector3.Cross(forwardDirection, upDirection).normalized;
            
            Matrix4x4 localToWorldMatrix = new Matrix4x4();
            localToWorldMatrix.SetColumn(0, rightDirection);
            localToWorldMatrix.SetColumn(1, upDirection);
            localToWorldMatrix.SetColumn(2, forwardDirection);
            localToWorldMatrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            
            return localToWorldMatrix.MultiplyPoint3x4(localDirection);
        }
        
        public static Vector3 RotatePivot(Vector3 pivot, Vector3 position, Quaternion rotation)
        {
            Vector3 direction = rotation * (position - pivot); 
            return pivot + direction;
        }
    }
}