using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class Aim
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_MinAccuracy = GetDecimalConstantZero.Create;
        
        [SerializeReference] private TAim m_Aim = new AimCameraDistance();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public TAim Value => this.m_Aim;
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public float GetMinAccuracy(Args args)
        {
            return (float) this.m_MinAccuracy.Get(args);
        }
        
        public Vector3 GetPoint(Args args)
        {
            return this.m_Aim.GetPoint(args);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Enter(Character character)
        {
            this.m_Aim.Enter(character);
        }

        public void Exit(Character character)
        {
            this.m_Aim.Exit(character);
        }
    }
}