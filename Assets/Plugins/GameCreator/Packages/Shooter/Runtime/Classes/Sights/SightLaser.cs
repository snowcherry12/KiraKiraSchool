using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class SightLaser
    {
        [SerializeField] private PropertyGetBool m_UseLaser = GetBoolFalse.Create;
        [SerializeField] private Vector3 m_Offset = Vector3.zero;
        
        [SerializeField] private EnablerLayerMask m_UseRaycast = new EnablerLayerMask(true);
        
        [SerializeField] private PropertyGetDecimal m_MaxDistance = new PropertyGetDecimal(10f);

        [SerializeField] private PropertyGetGameObject m_PrefabDot = GetGameObjectInstance.Create();
        [SerializeField] private PropertyGetMaterial m_Material = GetMaterialInstance.Create();
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsRed.Create;

        [SerializeField] private PropertyGetDecimal m_Width = GetDecimalDecimal.Create(0.005f);
        [SerializeField] private LineTextureMode m_TextureMode = LineTextureMode.Stretch;
        [SerializeField] private LineAlignment m_TextureAlign = LineAlignment.View;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        internal EnablerLayerMask UseRaycast => this.m_UseRaycast;

        internal LineTextureMode TextureMode => this.m_TextureMode;
        internal LineAlignment TextureAlign => this.m_TextureAlign;

        internal Vector3 Offset => this.m_Offset;
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal bool CanDraw(Args args)
        {
            return this.m_UseLaser.Get(args);
        }

        internal GameObject GetPrefabDot(Args args) => this.m_PrefabDot.Get(args);
        internal float GetMaxDistance(Args args) => (float) this.m_MaxDistance.Get(args);
        
        internal Material GetMaterial(Args args) => this.m_Material.Get(args);
        internal Color GetColor(Args args) => this.m_Color.Get(args);
        internal float GetWidth(Args args) => (float) this.m_Width.Get(args);
    }
}