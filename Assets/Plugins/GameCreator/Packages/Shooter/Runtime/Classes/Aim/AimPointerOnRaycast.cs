using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Pointer on Raycast")]
    [Description("Aims towards the point from the camera's point towards the first collider found")]

    [Category("Pointer on Raycast")]
    [Image(typeof(IconCursor), ColorTheme.Type.Yellow, typeof(OverlayPhysics))]

    [Serializable]
    public class AimPointerOnRaycast : TAim
    {
        private const float INFINITY = 999f;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;
        [SerializeField]
        private InputPropertyValueVector2 m_Cursor = new InputPropertyValueVector2(
            new InputValueVector2MousePosition()
        );

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectSelf.Create();

        [SerializeField] private PropertyGetDecimal m_MinDistance = GetDecimalConstantPointOne.Create;
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override Vector3 GetPoint(Args args)
        {
            Camera camera = this.m_Camera.Get<Camera>(args);
            if (camera == null) return default;

            Transform target = this.m_Target.Get<Transform>(args);
            if (target == null) return default;

            float minDistance = (float) this.m_MinDistance.Get(args);
            Vector2 cursor = this.m_Cursor.Read();

            bool isHit = Physics.Raycast(
                camera.ScreenPointToRay(cursor),
                out RaycastHit hitInfo,
                INFINITY,
                this.m_LayerMask
            );
            
            if (isHit)
            {
                float distance = hitInfo.distance;
                return distance >= minDistance 
                    ? hitInfo.point
                    : target.TransformPoint(Vector3.forward * INFINITY);
            }

            return target.TransformPoint(Vector3.forward * INFINITY);
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
