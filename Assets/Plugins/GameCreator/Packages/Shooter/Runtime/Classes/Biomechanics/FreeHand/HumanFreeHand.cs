using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class HumanFreeHand
    {
        private enum Attachment
        {
            ToWeapon,
            ToTransform,
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private HumanHand m_UseFreeHand = HumanHand.None;
        [SerializeField] private Attachment m_Attach = Attachment.ToWeapon;

        [SerializeField] private PropertyGetGameObject m_Transform = GetGameObjectInstance.Create();
        [SerializeField] private PropertyGetPosition m_Position = GetPositionVector3.Create(Vector3.zero);
        [SerializeField] private PropertyGetRotation m_Rotation = GetRotationConstantEulerVector.Create(Vector3.zero);
        [SerializeField] private PropertyGetGameObject m_Pole = GetGameObjectNone.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool UseFreeHand => this.m_UseFreeHand != HumanHand.None;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public Transform GetBone(Animator animator)
        {
            return this.m_UseFreeHand switch
            {
                HumanHand.None => null,
                HumanHand.LeftHand => animator.GetBoneTransform(HumanBodyBones.LeftHand),
                HumanHand.RightHand => animator.GetBoneTransform(HumanBodyBones.RightHand),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void GetHandle(
            Args args,
            out Vector3 position,
            out Quaternion rotation,
            out Transform target)
        {
            if (this.m_UseFreeHand == HumanHand.None)
            {
                position = Vector3.zero;
                rotation = Quaternion.identity;
                target = null;
                
                return;
            }
            
            position = this.m_Position.Get(args);
            rotation = this.m_Rotation.Get(args);
            target = this.m_Attach switch
            {
                Attachment.ToWeapon => args.Target.transform,
                Attachment.ToTransform => this.m_Transform.Get<Transform>(args),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Transform GetPole(Args args)
        {
            return this.m_Pole.Get<Transform>(args);
        }
    }
}