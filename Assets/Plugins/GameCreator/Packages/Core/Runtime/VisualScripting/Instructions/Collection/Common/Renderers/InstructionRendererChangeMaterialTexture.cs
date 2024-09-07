using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Change Material Texture")]
    [Description("Changes the main texture of an instantiated material of a Renderer component")]
    
    [Image(typeof(IconTexture), ColorTheme.Type.Yellow)]

    [Category("Renderer/Change Material Texture")]
    
    [Parameter("Texture", "Texture that replaces the Renderer's instantiated material")]

    [Keywords("Set", "Shader")]
    [Serializable]
    public class InstructionRendererChangeMaterialTexture : TInstructionRenderer
    {
        private enum MaterialType
        {
            Shared,
            Instance
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private MaterialType m_Material = MaterialType.Instance;
        [SerializeField] private PropertyGetString m_TextureName = new PropertyGetString("_MainTex");
        
        [SerializeField] private PropertyGetTexture m_Texture = new PropertyGetTexture();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title =>
            $"Change Texture of {this.m_Renderer}[{this.m_TextureName}] to {this.m_Texture}";
        
        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_Renderer.Get(args);
            if (gameObject == null) return DefaultResult;

            Renderer renderer = gameObject.Get<Renderer>();
            if (renderer == null) return DefaultResult;

            Material material = this.m_Material switch
            {
                MaterialType.Shared => renderer.sharedMaterial,
                MaterialType.Instance => renderer.material,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            if (material == null) return DefaultResult;

            string textureName = this.m_TextureName.Get(args);
            if (material.HasTexture(textureName) == false) return DefaultResult;
            
            material.SetTexture(textureName, this.m_Texture.Get(args));
            return DefaultResult;
        }
    }
}