using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Camera Center with Raycast")]
    [Description("Aims towards the first intersected object from the center of the screen")]
    
    [Category("Camera Center with Raycast")]
    [Image(typeof(IconCamera), ColorTheme.Type.Yellow, typeof(OverlayPhysics))]
    
    [Serializable]
    public class AimCameraRaycast : TAim
    {
        // CONSTANTS: -----------------------------------------------------------------------------

        private const float INFINITY = 999f; 
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;
        [SerializeField] private LayerMask m_LayerMask = Physics.DefaultRaycastLayers;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Vector3 GetPoint(Args args)
        {
            Vector3 defaultDirection = Vector3.forward * INFINITY;
            Transform camera = this.m_Camera.Get<Transform>(args);

            if (camera == null)
            {
                return args.Self.transform.TransformPoint(defaultDirection);
            }
            
            Vector3 cameraDirection = camera.TransformDirection(Vector3.forward);

            bool isHit = Physics.Raycast(
                camera.position,
                cameraDirection.normalized,
                out RaycastHit hit,
                INFINITY,
                this.m_LayerMask,
                QueryTriggerInteraction.Ignore
            );
            
            return isHit ? hit.point : camera.TransformPoint(defaultDirection);
        }
    }
}