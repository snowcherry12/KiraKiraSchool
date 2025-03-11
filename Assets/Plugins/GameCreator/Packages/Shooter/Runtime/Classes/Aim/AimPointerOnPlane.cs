using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Pointer on Plane")]
    [Description("Aims towards the point projected from the cursor towards the specified plane")]
    
    [Category("Pointer on Plane")]
    [Image(typeof(IconCursor), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class AimPointerOnPlane : TAim
    {
        private enum Axis
        {
            XZ,
            XY,
            YZ
        }

        private const float INFINITY = 999f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;
        [SerializeField] private InputPropertyValueVector2 m_Cursor = new InputPropertyValueVector2(
            new InputValueVector2MousePosition()
        );
        
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectSelf.Create();

        [SerializeField] private PropertyGetDecimal m_MinDistance = GetDecimalConstantPointOne.Create;
        [SerializeField] private Axis m_Plane = Axis.XZ;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Vector3 GetPoint(Args args)
        {
            Camera camera = this.m_Camera.Get<Camera>(args);
            if (camera == null) return default;
            
            Vector2 cursor = this.m_Cursor.Read();
            Ray ray = camera.ScreenPointToRay(cursor);
            
            Transform target = this.m_Target.Get<Transform>(args);
            if (target == null) return default;
            
            Plane plane = new Plane(
                this.m_Plane switch
                {
                    Axis.XZ => Vector3.up,
                    Axis.XY => Vector3.forward,
                    Axis.YZ => Vector3.right,
                    _ => throw new ArgumentOutOfRangeException()
                },
                target.position
            );
            
            float minDistance = (float) this.m_MinDistance.Get(args);
            return plane.Raycast(ray, out float distance) && distance >= minDistance
                ? ray.GetPoint(distance)
                : target.TransformPoint(Vector3.forward * INFINITY);
        }

        public override void Enter(Character character)
        {
            base.Enter(character);
            this.m_Cursor.OnStartup();
        }
        
        public override void Exit(Character character)
        {
            base.Exit(character);
            this.m_Cursor.OnDispose();
        }
    }
}