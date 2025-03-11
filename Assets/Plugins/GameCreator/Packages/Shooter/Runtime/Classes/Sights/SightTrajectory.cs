using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class SightTrajectory
    {
        [SerializeField] private PropertyGetBool m_UseTrajectory = GetBoolFalse.Create;
        [SerializeField] private EnablerLayerMask m_UseRaycast = new EnablerLayerMask(true);
        
        [SerializeField] private PropertyGetInteger m_MaxResolution = new PropertyGetInteger(32);
        [SerializeField] private PropertyGetDecimal m_MaxDistance = new PropertyGetDecimal(10f);

        [SerializeField] private PropertyGetMaterial m_Material = GetMaterialInstance.Create();
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;

        [SerializeField] private PropertyGetInteger m_CornerVertices = new PropertyGetInteger(new GetDecimalConstantTwo());
        [SerializeField] private PropertyGetInteger m_CapVertices = new PropertyGetInteger(new GetDecimalConstantZero());
        
        [SerializeField] private PropertyGetDecimal m_Width = GetDecimalConstantPointOne.Create;
        [SerializeField] private LineTextureMode m_TextureMode = LineTextureMode.Stretch;
        [SerializeField] private LineAlignment m_TextureAlign = LineAlignment.View;
        
        [SerializeField] private PropertyGetGameObject m_PrefabDot = GetGameObjectInstance.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        internal EnablerLayerMask UseRaycast => this.m_UseRaycast;

        internal LineTextureMode TextureMode => this.m_TextureMode;
        internal LineAlignment TextureAlign => this.m_TextureAlign;
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal bool CanDraw(Args args)
        {
            return this.m_UseTrajectory.Get(args);
        }

        internal int GetMaxResolution(Args args) => (int) this.m_MaxResolution.Get(args);
        internal float GetMaxDistance(Args args) => (float) this.m_MaxDistance.Get(args);

        internal int GetCornerVertices(Args args) => (int) this.m_CornerVertices.Get(args);
        internal int GetCapVertices(Args args) => (int) this.m_CapVertices.Get(args);
        
        internal Material GetMaterial(Args args) => this.m_Material.Get(args);
        internal Color GetColor(Args args) => this.m_Color.Get(args);
        internal float GetWidth(Args args) => (float) this.m_Width.Get(args);

        internal GameObject GetPrefabDot(Args args) => this.m_PrefabDot.Get(args);
    }
}