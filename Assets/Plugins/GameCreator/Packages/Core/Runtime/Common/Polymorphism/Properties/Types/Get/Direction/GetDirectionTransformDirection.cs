using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Transform Direction")]
    [Category("Transforms/Transform Direction")]
    
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("Transforms the local space direction to world space and returns the value")]
    
    [Serializable]
    public class GetDirectionTransformDirection : PropertyTypeGetDirection
    {
        [SerializeField] protected PropertyGetGameObject m_From = GetGameObjectPlayer.Create();
        [SerializeField] protected PropertyGetDirection m_Direction = GetDirectionVector.Create();
        
        public override Vector3 Get(Args args)
        {
            GameObject from = this.m_From.Get(args);
            Vector3 direction = this.m_Direction.Get(args);
            
            return from != null ? from.transform.TransformDirection(direction) : direction;
        }

        public GetDirectionTransformDirection()
        { }
        
        public GetDirectionTransformDirection(Vector3 direction)
        {
            this.m_Direction = GetDirectionVector.Create(direction);
        }
        
        public static PropertyGetDirection Create(Vector3 direction) => new PropertyGetDirection(
            new GetDirectionTransformDirection(direction)
        );
        
        public static PropertyGetDirection Create(PropertyGetGameObject from, PropertyGetDirection direction)
        {
            return new PropertyGetDirection(
                new GetDirectionTransformDirection
                {
                    m_From = from,
                    m_Direction = direction
                }
            );
        }

        public override string String => $"{this.m_From} {this.m_Direction}";
    }
}