using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [CreateAssetMenu(
        fileName = "Material Sounds", 
        menuName = "Game Creator/Common/Material Sounds",
        order = 50
    )]
    [Icon(RuntimePaths.GIZMOS + "GizmoMaterialSounds.png")]
    public class MaterialSoundsAsset : ScriptableObject
    {
        private const string MAIN_TEXTURE = "_MainTex";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private string m_TextureName = MAIN_TEXTURE;
        [SerializeField] private MaterialSoundsData m_MaterialSounds = new MaterialSoundsData();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private int m_TextureID;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int TextureID
        {
            get
            {
                if (this.m_TextureID == 0)
                {
                    this.m_TextureID = string.IsNullOrEmpty(this.m_TextureName) == false
                        ? Shader.PropertyToID(this.m_TextureName)
                        : Shader.PropertyToID(MAIN_TEXTURE);
                }

                #if UNITY_EDITOR
                
                int textureID = string.IsNullOrEmpty(this.m_TextureName) == false
                    ? Shader.PropertyToID(this.m_TextureName)
                    : Shader.PropertyToID(MAIN_TEXTURE);
                
                this.m_TextureID = textureID;
                
                #endif
                
                return this.m_TextureID;
            }
        }

        public MaterialSoundsData MaterialSounds => m_MaterialSounds;
    }
}
