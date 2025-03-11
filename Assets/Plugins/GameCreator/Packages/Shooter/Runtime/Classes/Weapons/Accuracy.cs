using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Accuracy
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetDecimal m_MaxSpreadX = GetDecimalConstantTwo.Create;
        [SerializeField] private PropertyGetDecimal m_MaxSpreadY = GetDecimalConstantTwo.Create;
        [SerializeField] private PropertyGetDecimal m_SpreadBias = GetDecimalConstantOne.Create;
        
        [SerializeField]
        private PropertyGetDecimal m_AccuracyRecover = GetDecimalConstantOne.Create;

        [SerializeField]
        private PropertyGetDecimal m_MotionAccuracy = GetDecimalConstantPointFive.Create;
        
        [SerializeField]
        private PropertyGetDecimal m_AirborneAccuracy = GetDecimalConstantPointFive.Create;
        
        [SerializeField] 
        private PropertyGetDecimal m_AccuracyKick = GetDecimalDecimal.Create(0.25f);
        
        // INTERNAL METHODS: ----------------------------------------------------------------------
        
        internal float GetAccuracyRecover(Args args)
        {
            return (float) this.m_AccuracyRecover.Get(args);
        }

        internal void OnShoot(Args args)
        {
            ShooterStance stance = args.Self.Get<Character>().Combat.RequestStance<ShooterStance>();
            
            float kick = (float) this.m_AccuracyKick.Get(args);
            stance.AccuracyKick(kick);
        }
        
        internal float CalculateTargetAccuracy(ShooterStance stance, WeaponData weaponData)
        {
            if (stance.Reloading.WeaponReloading == weaponData.Weapon) return 1f;

            float motionAccuracy = this.GetAccuracyFromMotion(stance, weaponData);
            float airborneAccuracy = this.GetAccuracyFromAirborne(stance, weaponData);

            Sight sight = weaponData.Weapon != null
                ? weaponData.Weapon.Sights.Get(weaponData.SightId)?.Sight
                : null;
            
            float minAccuracy = sight != null
                ? sight.Aim.GetMinAccuracy(weaponData.WeaponArgs)
                : 0f;
            
            float accuracy = MathUtils.Max(
                minAccuracy,
                motionAccuracy,
                airborneAccuracy
            );
            
            return Mathf.Clamp01(accuracy);
        }
        
        internal Vector2 CalculateSpread(Args args)
        {
            ShooterStance stance = args.Self.Get<Character>().Combat.RequestStance<ShooterStance>();

            Vector2 spread = Vector2.Scale(
                new Vector2(
                    UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(-1f, 1f)
                ).normalized,
                new Vector2(
                    (float) this.m_MaxSpreadX.Get(args),
                    (float) this.m_MaxSpreadY.Get(args)
                )
            );
            
            float bias = Mathf.Clamp01((float) this.m_SpreadBias.Get(args));
            float radius = -Mathf.Pow(UnityEngine.Random.value, bias) + 1f;
            
            return spread * (radius * stance.CurrentAccuracy);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private float GetAccuracyFromMotion(IStance stance, WeaponData weaponData)
        {
            float motionAccuracy = (float) this.m_MotionAccuracy.Get(weaponData.WeaponArgs);
            float motionCurrent = stance.Character.Driver.WorldMoveDirection.magnitude;
            float motionMaximum = stance.Character.Motion.LinearSpeed;
            
            return Mathf.Clamp01(motionCurrent / motionMaximum) * motionAccuracy;
        }
        
        private float GetAccuracyFromAirborne(IStance stance, WeaponData weaponData)
        {
            float airborneAccuracy = (float) this.m_AirborneAccuracy.Get(weaponData.WeaponArgs);
            return stance.Character.Driver.IsGrounded ? 0 : airborneAccuracy;
        }
    }
}