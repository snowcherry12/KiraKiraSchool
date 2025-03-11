using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Camera Center at Distance")]
    [Description("Aims towards a point at a distance from the center of the camera")]
    
    [Category("Camera Center at Distance")]
    [Image(typeof(IconCamera), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class AimCameraDistance : TAim
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;
        [SerializeField] private PropertyGetDecimal m_ZeroDistance = GetDecimalDecimal.Create(10f);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Vector3 GetPoint(Args args)
        {
            Transform camera = this.m_Camera.Get<Transform>(args);
            if (camera == null) return default;
            
            float zeroDistance = (float) this.m_ZeroDistance.Get(args);
            return camera.TransformPoint(Vector3.forward * zeroDistance);
        }
    }
}