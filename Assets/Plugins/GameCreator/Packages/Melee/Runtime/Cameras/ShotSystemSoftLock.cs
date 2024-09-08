using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ShotSystemSoftLock : TShotSystem
    {
        public static readonly int ID = nameof(ShotSystemSoftLock).GetHashCode();
        
        protected static readonly Vector3 GIZMO_SIZE_CUBE = Vector3.one * 0.1f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private InputPropertyValueVector2 m_InputOrbit;

        [SerializeField] private float m_MinDistance = 1f;
        [SerializeField] private float m_MaxDistance = 3f;
        
        [SerializeField] private float m_Height = 0.5f;
        [SerializeField] private float m_Radius = 3f;

        [SerializeField] private float m_ExtraHeight = 2f;
        [SerializeField] private float m_ExtraRadius = 3f;

        [SerializeField]
        private PropertyGetDecimal m_InputSensitivityX = GetDecimalDecimal.Create(180f);
        
        [SerializeField]
        private PropertyGetDecimal m_InputSensitivityY = GetDecimalDecimal.Create(180f);

        [SerializeField, Range(1f, 179f)] private float m_MaxPitch = 60f;
        
        [SerializeField] private float m_SmoothTime = 0.5f;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Vector3 m_LastTargetPosition = Vector3.zero;
        
        [NonSerialized] private Vector2 m_OrbitAnglesCurrent = new Vector2(45f, 0f);
        [NonSerialized] private Vector2 m_OrbitAnglesTarget = new Vector2(45f, 0f);

        [NonSerialized] private float m_CurrentRatio;
        [NonSerialized] private float m_RatioVelocity;

        [NonSerialized] private float m_VelocityX;
        [NonSerialized] private float m_VelocityY;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Id => ID;

        public float MaxPitch
        {
            get => this.m_MaxPitch;
            set => this.m_MaxPitch = value;
        }

        public float Pitch
        {
            get => this.m_OrbitAnglesTarget.x;
            set => this.m_OrbitAnglesTarget.x = value;
        }
        
        public float Yaw
        {
            get => this.m_OrbitAnglesTarget.y;
            set => this.m_OrbitAnglesTarget.y = value;
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotSystemSoftLock()
        {
            this.m_InputOrbit = InputValueVector2MotionSecondary.Create();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void SetRotation(float pitch, float yaw)
        {
            Vector2 rotation = new Vector2(pitch, yaw);
            
            this.m_OrbitAnglesTarget = rotation;
            this.m_OrbitAnglesCurrent = rotation;
        }

        public void SetDirection(Vector3 direction)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            this.SetRotation(rotation.eulerAngles.x, rotation.eulerAngles.y);
        }

        // IMPLEMENTS: ----------------------------------------------------------------------------
        
        public override void OnAwake(TShotType shotType)
        {
            base.OnAwake(shotType);
            this.m_InputOrbit.OnStartup();
        }

        public override void OnDestroy(TShotType shotType)
        {
            base.OnDestroy(shotType);
            this.m_InputOrbit.OnDispose();
        }

        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);
            this.m_InputOrbit.OnUpdate();

            if (shotType.IsActive)
            {
                Vector2 deltaInput = this.m_InputOrbit.Read();

                float sensitivityX = (float) this.m_InputSensitivityX.Get(shotType.Args);
                float sensitivityY = (float) this.m_InputSensitivityY.Get(shotType.Args);
                
                if (deltaInput != Vector2.zero)
                {
                    this.ComputeInput(shotType, new Vector2(
                        deltaInput.x * shotType.ShotCamera.TimeMode.DeltaTime * sensitivityX, 
                        deltaInput.y * shotType.ShotCamera.TimeMode.DeltaTime * sensitivityY
                    ));
                }
            }

            this.m_CurrentRatio = Mathf.SmoothDamp(
                this.m_CurrentRatio,
                this.ComputeRatio(shotType as TShotTypeLook),
                ref this.m_RatioVelocity,
                this.m_SmoothTime,
                Mathf.Infinity,
                shotType.ShotCamera.TimeMode.DeltaTime
            );
            
            float radius = Mathf.Lerp(
                this.m_Radius,
                this.m_Radius + this.m_ExtraRadius,
                this.m_CurrentRatio
            );

            float height = Mathf.Lerp(
                this.m_Height,
                this.m_Height + this.m_ExtraHeight,
                this.m_CurrentRatio
            );

            this.ConstrainTargetAngles();

            this.m_OrbitAnglesCurrent = new Vector2(
                this.GetRotationDamp(
                    this.m_OrbitAnglesCurrent.x, 
                    this.m_OrbitAnglesTarget.x,
                    ref this.m_VelocityX,
                    this.m_SmoothTime,
                    shotType.ShotCamera.TimeMode.DeltaTime),
                this.GetRotationDamp(
                    this.m_OrbitAnglesCurrent.y, 
                    this.m_OrbitAnglesTarget.y, 
                    ref this.m_VelocityY,
                    this.m_SmoothTime,
                    shotType.ShotCamera.TimeMode.DeltaTime)
            );
            
            this.m_LastTargetPosition = this.GetTargetPosition(shotType as TShotTypeLook, height);
            
            Quaternion lookRotation = Quaternion.Euler(this.m_OrbitAnglesCurrent);
            Vector3 lookDirection = lookRotation * Vector3.forward;

            Vector3 position = this.m_LastTargetPosition - lookDirection * radius;
            shotType.Position = position;
        }

        public override void OnEnable(TShotType shotType, TCamera camera)
        {
            base.OnEnable(shotType, camera);

            float height = Mathf.Lerp(
                this.m_Height,
                this.m_Height + this.m_ExtraHeight,
                this.m_CurrentRatio
            );
            
            Vector3 targetPosition = this.GetTargetPosition(shotType as TShotTypeLook, height);
            Vector3 targetDirection = targetPosition - camera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            
            float pitch = targetRotation.eulerAngles.x;
            if (pitch > 180f) pitch -= 360f;
            
            float angle = this.m_MaxPitch / 2f;
            pitch = Mathf.Clamp(pitch, -angle, angle);
            
            float yaw = camera.transform.eulerAngles.y;
            
            this.SetRotation(pitch, yaw);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public override void OnDrawGizmosSelected(TShotType shotType, Transform transform)
        {
            base.OnDrawGizmosSelected(shotType, transform);
            this.DoDrawGizmos(shotType, GIZMOS_COLOR_ACTIVE);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private float GetRotationDamp(float current, float target, ref float velocity, 
            float smoothTime, float deltaTime)
        {
            return Mathf.SmoothDampAngle(
                current,
                target,
                ref velocity,
                smoothTime,
                Mathf.Infinity,
                deltaTime
            );
        }
        
        private void ConstrainTargetAngles()
        {
            float angle = this.m_MaxPitch / 2f;
            m_OrbitAnglesTarget.x = Mathf.Clamp(m_OrbitAnglesTarget.x, -angle, angle);

            if (m_OrbitAnglesTarget.y < 0f) m_OrbitAnglesTarget.y += 360f;
            if (m_OrbitAnglesTarget.y >= 360f) m_OrbitAnglesTarget.y -= 360f;
        }
        
        private Vector3 GetTargetPosition(TShotTypeLook shotTypeLook, float height)
        {
            if (shotTypeLook.Look.GetLookTarget(shotTypeLook) == null)
            {
                return this.m_LastTargetPosition;
            }
            
            Vector3 position = shotTypeLook.Look.GetLookPosition(shotTypeLook);
            return position + Vector3.up * height;
        }
        
        private void ComputeInput(TShotType shotType, Vector2 deltaInput)
        {
            this.m_OrbitAnglesTarget += new Vector2(
                deltaInput.y,
                deltaInput.x
            );
        }
        
        private float ComputeRatio(TShotTypeLook shotTypeLook)
        {
            if (shotTypeLook == null) return 0;
            
            Transform source = shotTypeLook.Look.GetLookTarget(shotTypeLook);
            Transform target = source;

            if (source != null)
            {
                Character character = source.Get<Character>();
                if (character != null) target = character.Combat.Targets.Primary != null
                    ? character.Combat.Targets.Primary.transform
                    : source;
            }

            if (source == null) return 0f;
            if (target == null) return 0f;
            
            Vector3 targetsVector = target.position - source.position;
            Vector3 cameraProjection = Vector3.ProjectOnPlane(
                targetsVector,
                shotTypeLook.ShotCamera.transform.forward
            );
            
            return Mathf.InverseLerp(
                this.m_MinDistance,
                this.m_MaxDistance,
                cameraProjection.magnitude
            );
        }
        
        private void DoDrawGizmos(TShotType shotType, Color color)
        {
            if (!Application.isPlaying) return;
            
            Gizmos.color = color;
            
            if (shotType is not ShotTypeSoftLock shotTypeSoftLock) return;

            Transform source = shotTypeSoftLock.Look.GetLookTarget(shotType);
            if (source != null)
            {
                Gizmos.DrawWireCube(source.position, GIZMO_SIZE_CUBE);
                Character character = source.Get<Character>();

                if (character != null && character.Combat.Targets.Primary != null)
                {
                    Transform target = character.Combat.Targets.Primary.transform;
                    
                    Gizmos.DrawLine(source.position, target.position);
                    Gizmos.DrawWireCube(target.position, GIZMO_SIZE_CUBE);
                }
            }
            
            Vector3 center = shotTypeSoftLock.Look.GetLookPosition(shotType);

            GizmosExtension.Circle(center, this.m_Radius * 2f);
            GizmosExtension.Circle(center, (this.m_Radius + this.m_ExtraRadius) * 2f);

            float currentRadius = Mathf.Lerp(
                this.m_Radius,
                this.m_Radius + this.m_ExtraRadius,
                this.m_CurrentRatio
            );
            
            GizmosExtension.Circle(center, currentRadius * 2f);
        }
    }
}