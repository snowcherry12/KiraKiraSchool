using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Kinematic")]
    [Image(typeof(IconCharacterRun), ColorTheme.Type.Green)]
    
    [Category("Kinematic")]
    [Description("Default animation system for characters")]

    [Serializable]
    public class UnitAnimimKinematic : TUnitAnimim
    {
        private const float DECAY_PIVOT = 5f;
        private const float DECAY_GROUNDED = 10f;
        private const float DECAY_STAND = 5f;
        
        // STATIC PROPERTIES: ---------------------------------------------------------------------
        
        private static readonly int K_SPEED_X = Animator.StringToHash("Speed-X");
        private static readonly int K_SPEED_Y = Animator.StringToHash("Speed-Y");
        private static readonly int K_SPEED_Z = Animator.StringToHash("Speed-Z");
        private static readonly int K_SPEED_XZ = Animator.StringToHash("Speed-XZ");
        private static readonly int K_SPEED_YZ = Animator.StringToHash("Speed-YZ");
        private static readonly int K_SPEED_XY = Animator.StringToHash("Speed-XY");
        
        private static readonly int K_INTENT_X = Animator.StringToHash("Intent-X");
        private static readonly int K_INTENT_Y = Animator.StringToHash("Intent-Y");
        private static readonly int K_INTENT_Z = Animator.StringToHash("Intent-Z");
        
        private static readonly int K_SPEED = Animator.StringToHash("Speed");
        private static readonly int K_PIVOT_SPEED = Animator.StringToHash("Pivot");

        private static readonly int K_GROUNDED = Animator.StringToHash("Grounded");
        private static readonly int K_STAND = Animator.StringToHash("Stand");

        // UPDATE METHOD: -------------------------------------------------------------------------

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (this.m_Animator == null) return;
            if (!this.m_Animator.gameObject.activeInHierarchy) return;

            this.m_Animator.updateMode = this.Character.Time.UpdateTime == TimeMode.UpdateMode.GameTime
                ? AnimatorUpdateMode.Normal
                : AnimatorUpdateMode.UnscaledTime;

            IUnitMotion motion = this.Character.Motion;
            IUnitDriver driver = this.Character.Driver;
            IUnitFacing facing = this.Character.Facing;

            Vector3 intent = motion.LinearSpeed > float.Epsilon
                ? Vector3.ClampMagnitude(this.Transform.InverseTransformDirection(motion.MoveDirection) / motion.LinearSpeed, 1f)
                : Vector3.zero;
            
            Vector3 speed = motion.LinearSpeed > float.Epsilon
                ? driver.LocalMoveDirection / motion.LinearSpeed
                : Vector3.zero;
            
            float pivot = facing.PivotSpeed;

            float deltaTime = this.Character.Time.DeltaTime;
            float decay = Mathf.Lerp(1f, 25f, this.m_SmoothTime);
            
            this.m_Animator.SetFloat(K_SPEED_X, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED_X), speed.x, decay, deltaTime));
            this.m_Animator.SetFloat(K_SPEED_Y, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED_Y), speed.y, decay, deltaTime));
            this.m_Animator.SetFloat(K_SPEED_Z, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED_Z), speed.z, decay, deltaTime));
            this.m_Animator.SetFloat(K_SPEED, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED), speed.magnitude, decay, deltaTime));
            this.m_Animator.SetFloat(K_SPEED_XZ, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED_XZ), speed.XZ().magnitude, decay, deltaTime));
            this.m_Animator.SetFloat(K_SPEED_XY, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED_XY), speed.XY().magnitude, decay, deltaTime));
            this.m_Animator.SetFloat(K_SPEED_YZ, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_SPEED_YZ), speed.YZ().magnitude, decay, deltaTime));
            this.m_Animator.SetFloat(K_INTENT_X, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_INTENT_X), intent.x, decay, deltaTime));
            this.m_Animator.SetFloat(K_INTENT_Y, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_INTENT_Y), intent.y, decay, deltaTime));
            this.m_Animator.SetFloat(K_INTENT_Z, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_INTENT_Z), intent.z, decay, deltaTime));
            
            this.m_Animator.SetFloat(K_PIVOT_SPEED, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_PIVOT_SPEED), pivot, DECAY_PIVOT, deltaTime));
            this.m_Animator.SetFloat(K_GROUNDED, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_GROUNDED), driver.IsGrounded ? 1f : 0f, DECAY_GROUNDED, deltaTime));
            this.m_Animator.SetFloat(K_STAND, MathUtils.ExponentialDecay(this.m_Animator.GetFloat(K_STAND), motion.StandLevel.Current, DECAY_STAND, deltaTime));
        }
    }
}