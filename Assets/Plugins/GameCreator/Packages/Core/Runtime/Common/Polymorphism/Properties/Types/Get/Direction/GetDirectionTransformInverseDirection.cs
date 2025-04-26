using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Inverse Transform Direction")]
    [Category("Transforms/Inverse Transform Direction")]
    
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Green, typeof(OverlayArrowLeft))]
    [Description("Transforms the world space direction to local space and returns the value")]
    
    [Serializable]
    public class GetDirectionTransformInverseDirection : PropertyTypeGetDirection
    {
        [SerializeField] protected PropertyGetDirection m_Direction = GetDirectionVector.Create(Vector3.forward);
        [SerializeField] protected PropertyGetGameObject m_To = GetGameObjectPlayer.Create();
        
        public override Vector3 Get(Args args)
        {
            GameObject to = this.m_To.Get(args);
            Vector3 direction = this.m_Direction.Get(args);
            
            return to != null ? to.transform.InverseTransformDirection(direction) : direction;
        }
        
        public static PropertyGetDirection Create(PropertyGetDirection direction, PropertyGetGameObject to)
        {
            return new PropertyGetDirection(
                new GetDirectionTransformInverseDirection
                {
                    m_Direction = direction,
                    m_To = to
                }
            );
        }

        public static PropertyGetDirection Create() => new PropertyGetDirection(
            new GetDirectionTransformInverseDirection()
        );

        public override string String => $"{this.m_To} {this.m_Direction}";
    }
}