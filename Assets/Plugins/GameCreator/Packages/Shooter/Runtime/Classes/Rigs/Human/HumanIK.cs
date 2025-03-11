using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    internal class HumanIK
    {
        private struct Pose
        {
            public Vector3 Position { get; private set; }
            public Quaternion Rotation { get; private set; }

            public void Set(Vector3 position, Quaternion rotation)
            {
                this.Position = position;
                this.Rotation = rotation;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        // CONSTANTS: -----------------------------------------------------------------------------
        
        private const float DECAY = 0.15f;
        
        private const float MAX = 1f;
        private const float MIN = 0f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly Transform m_Mannequin;
        
        [NonSerialized] private readonly Transform m_UpperArm;
        [NonSerialized] private readonly Transform m_LowerArm;
        [NonSerialized] private readonly Transform m_Hand;
        
        [NonSerialized] private readonly SpringFloat m_UseIK = new SpringFloat(MIN, DECAY);
        
        [NonSerialized] private Pose m_LastLocalUpperArm;
        [NonSerialized] private Pose m_LastLocalLowerArm;
        [NonSerialized] private Pose m_LastLocalHand;
        
        [NonSerialized] private Pose m_TargetUpperArm;
        [NonSerialized] private Pose m_TargetLowerArm;
        [NonSerialized] private Pose m_TargetHand;
        
        [NonSerialized] private bool m_IsInitialized;
        [NonSerialized] private Sight m_LastSight;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public HumanIK(Transform mannequin, Transform hand)
        {
            this.m_IsInitialized = false;
            this.m_LastSight = null;

            this.m_Mannequin = mannequin;
            
            this.m_UpperArm = hand.parent.parent;
            this.m_LowerArm = hand.parent;
            this.m_Hand = hand;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Update(Sight sight, TwoBoneData twoBoneData, float deltaTime, float ratio)
        {
            this.m_TargetUpperArm.Set(
                Vector3.Lerp(this.m_UpperArm.position, twoBoneData.RootPosition, ratio),
                Quaternion.Lerp(this.m_UpperArm.rotation, twoBoneData.RootRotation, ratio)
            );
            
            this.m_TargetLowerArm.Set(
                Vector3.Lerp(this.m_LowerArm.position, twoBoneData.BodyPosition, ratio),
                Quaternion.Lerp(this.m_LowerArm.rotation, twoBoneData.BodyRotation, ratio)
            );
            
            this.m_TargetHand.Set(
                Vector3.Lerp(this.m_Hand.position, twoBoneData.HeadPosition, ratio),
                Quaternion.Lerp(this.m_Hand.rotation, twoBoneData.HeadRotation, ratio)
            );
            
            if (sight != null && sight != this.m_LastSight)
            // if (this.m_LastSight != null && sight != this.m_LastSight)
            {
                this.m_UseIK.Current = MIN;
                this.m_LastSight = sight;
            }
            
            if (this.m_IsInitialized == false)
            {
                this.m_LastLocalUpperArm.Set(
                    this.m_Mannequin.InverseTransformPoint(this.m_UpperArm.position),
                    Quaternion.Inverse(this.m_Mannequin.rotation) * this.m_UpperArm.rotation
                );
            
                this.m_LastLocalLowerArm.Set(
                    this.m_Mannequin.InverseTransformPoint(this.m_LowerArm.position),
                    Quaternion.Inverse(this.m_Mannequin.rotation) * this.m_LowerArm.rotation
                );
            
                this.m_LastLocalHand.Set(
                    this.m_Mannequin.InverseTransformPoint(this.m_Hand.position),
                    Quaternion.Inverse(this.m_Mannequin.rotation) * this.m_Hand.rotation
                );
                
                this.m_IsInitialized = true;
            }
            
            float t = this.m_UseIK.Update(MAX, DECAY, deltaTime);
            
            Vector3 upperArmPosition = Vector3.Lerp(
                this.m_Mannequin.TransformPoint(this.m_LastLocalUpperArm.Position),
                this.m_TargetUpperArm.Position, 
                t
            );

            Quaternion upperArmRotation = Quaternion.Lerp(
                this.m_Mannequin.rotation * this.m_LastLocalUpperArm.Rotation,
                this.m_TargetUpperArm.Rotation,
                t
            );
            
            Vector3 lowerArmPosition = Vector3.Lerp(
                this.m_Mannequin.TransformPoint(this.m_LastLocalLowerArm.Position),
                this.m_TargetLowerArm.Position, 
                t
            );
            
            Quaternion lowerArmRotation = Quaternion.Lerp(
                this.m_Mannequin.rotation * this.m_LastLocalLowerArm.Rotation,
                this.m_TargetLowerArm.Rotation,
                t
            );
            
            Vector3 handPosition = Vector3.Lerp(
                this.m_Mannequin.TransformPoint(this.m_LastLocalHand.Position),
                this.m_TargetHand.Position, 
                t
            );
            
            Quaternion handRotation = Quaternion.Lerp(
                this.m_Mannequin.rotation * this.m_LastLocalHand.Rotation,
                this.m_TargetHand.Rotation,
                t
            );
            
            this.m_UpperArm.position = upperArmPosition;
            this.m_UpperArm.rotation = upperArmRotation;
            
            this.m_LowerArm.position = lowerArmPosition;
            this.m_LowerArm.rotation = lowerArmRotation;
            
            this.m_Hand.position = handPosition;
            this.m_Hand.rotation = handRotation;
            
            this.m_LastLocalUpperArm.Set(
                this.m_Mannequin.InverseTransformPoint(upperArmPosition),
                Quaternion.Inverse(this.m_Mannequin.rotation) * upperArmRotation
            );
            
            this.m_LastLocalLowerArm.Set(
                this.m_Mannequin.InverseTransformPoint(lowerArmPosition),
                Quaternion.Inverse(this.m_Mannequin.rotation) * lowerArmRotation
            );
            
            this.m_LastLocalHand.Set(
                this.m_Mannequin.InverseTransformPoint(handPosition),
                Quaternion.Inverse(this.m_Mannequin.rotation) * handRotation
            );
        }
    }
}