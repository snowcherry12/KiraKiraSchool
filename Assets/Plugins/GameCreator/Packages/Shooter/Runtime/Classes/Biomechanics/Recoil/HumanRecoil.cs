using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class HumanRecoil
    {
        [SerializeField] private PropertyGetDecimal m_ImpactSpeed = GetDecimalDecimal.Create(0.02f);
        [SerializeField] private PropertyGetDecimal m_RecoverSpeed = GetDecimalConstantPointOne.Create;
        
        [SerializeField] private PropertyGetDecimal m_PositionX = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetDecimal m_PositionY = GetDecimalConstantZero.Create;
        
        [SerializeField] private PropertyGetDecimal m_RotationX = GetDecimalDecimal.Create(-10f);
        [SerializeField] private PropertyGetDecimal m_RotationY = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetDecimal m_RotationZ = GetDecimalConstantZero.Create;
        
        [SerializeField] private PropertyGetDecimal m_Kickback = GetDecimalConstantPointOne.Create;
        
        [SerializeField] private PropertyGetDecimal m_UpperArm = GetDecimalConstantZero.Create;
        [SerializeField] private PropertyGetDecimal m_LowerArm = GetDecimalDecimal.Create(0.05f);
        [SerializeField] private PropertyGetDecimal m_Hand = GetDecimalConstantPointFive.Create;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float GetImpactSpeed(Args args)
        {
            return (float) this.m_ImpactSpeed.Get(args);
        }
        
        public float GetRecoverSpeed(Args args)
        {
            return (float) this.m_RecoverSpeed.Get(args);
        }
        
        public Vector2 GetPosition(Args args)
        {
            return new Vector2(
                (float) this.m_PositionX.Get(args),
                (float) this.m_PositionY.Get(args)
            );
        }
        
        public Vector3 GetRotation(Args args)
        {
            return new Vector3(
                (float) this.m_RotationX.Get(args),
                (float) this.m_RotationY.Get(args),
                (float) this.m_RotationZ.Get(args)
            );
        }
        
        public float GetKickback(Args args)
        {
            return (float) this.m_Kickback.Get(args) * -1f;
        }
        
        public float GetRatioUpperArm(Args args)
        {
            return (float) this.m_UpperArm.Get(args);
        }

        public float GetRatioLowerArm(Args args)
        {
            return (float) this.m_LowerArm.Get(args);
        }

        public float GetRatioHands(Args args)
        {
            return (float) this.m_Hand.Get(args);
        }
    }
}