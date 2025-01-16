using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ReflectRenderPass : ScriptableRendererFeature
{
    private Material passMaterial;
    private Material compositeMaterial;
    public RenderPassEvent renderPass = RenderPassEvent.BeforeRenderingPostProcessing;
    private ScriptableRenderPassInput requirements = ScriptableRenderPassInput.Color;
    [HideInInspector] // We draw custom pass index entry with the dropdown inside FullScreenPassRendererFeatureEditor.cs
    public int passIndex = 0;

    private SSR_Pass ssrPass;
    private Composite_Pass compositePass;
    private bool requiresColor;
    private bool injectedBeforeTransparents;
    private bool isEnabled;

    public override void Create()
    {
        ssrPass = new SSR_Pass();
        ssrPass.renderPassEvent = renderPass;

        compositePass = new Composite_Pass();
        compositePass.renderPassEvent = renderPass;

        // This copy of requirements is used as a parameter to configure input in order to avoid copy color pass
        ScriptableRenderPassInput modifiedRequirements = requirements;

        requiresColor = (requirements & ScriptableRenderPassInput.Color) != 0;
        injectedBeforeTransparents = renderPass <= RenderPassEvent.BeforeRenderingTransparents;

        if (requiresColor && !injectedBeforeTransparents)
        {
            // Removing Color flag in order to avoid unnecessary CopyColor pass
            // Does not apply to before rendering transparents, due to how depth and color are being handled until
            // that injection point.
            modifiedRequirements ^= ScriptableRenderPassInput.Color;
        }
        ssrPass.ConfigureInput(modifiedRequirements);
        compositePass.ConfigureInput(modifiedRequirements);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        ScreenSpaceReflections ssr = VolumeManager.instance.stack.GetComponent<ScreenSpaceReflections>();
        isEnabled = ssr.IsActive();

        if (!renderingData.postProcessingEnabled)
        {
            isEnabled = false;
        }

        if (!isEnabled) { return; }

        if (passMaterial == null)
        {
            passMaterial = (Material)Resources.Load("SSR_Renderer");
        }
        if (compositeMaterial == null)
        {
            compositeMaterial = (Material)Resources.Load("SSR_Composite");
        }

        if (passMaterial == null || compositeMaterial == null)
        {
            Debug.LogWarningFormat("Missing Post Processing effect Material. {0} Fullscreen pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            return;
        }

        passMaterial.SetFloat("_Samples", ssr.steps.value);
        passMaterial.SetFloat("_BinarySamples", ssr.samples.value);
        passMaterial.SetFloat("_StepSize", ssr.stepSize.value);
        passMaterial.SetFloat("_Thickness", ssr.thickness.value);
        passMaterial.SetFloat("_MinSmoothness", ssr.minSmoothness.value);

        ssrPass.Setup(passMaterial, passIndex, requiresColor, injectedBeforeTransparents, "SSR", ssr.downsample.value, renderingData);
        compositePass.Setup(compositeMaterial, passIndex, requiresColor, injectedBeforeTransparents, "Comp", renderingData);

        renderer.EnqueuePass(ssrPass);
        renderer.EnqueuePass(compositePass);
    }

    protected override void Dispose(bool disposing)
    {
        ssrPass.Dispose();
        compositePass.Dispose();
    }

    class SSR_Pass : ScriptableRenderPass
    {
        private Material m_PassMaterial;
        private int m_PassIndex;
        private bool m_RequiresColor;
        private bool m_IsBeforeTransparents;
        private PassData m_PassData;
        private RTHandle m_CopiedColor;
        private static readonly int m_BlitTextureShaderID = Shader.PropertyToID("_BlitTexture");
        private RTHandle ScreenSpaceRelfectionsTex;
        private int downSample;

        public void Setup(Material mat, int index, bool requiresColor, bool isBeforeTransparents, string featureName, int ds, in RenderingData renderingData)
        {
            m_PassMaterial = mat;
            m_PassIndex = index;
            m_RequiresColor = requiresColor;
            m_IsBeforeTransparents = isBeforeTransparents;

            var colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            colorCopyDescriptor.depthBufferBits = (int) DepthBits.None;
            RenderingUtils.ReAllocateIfNeeded(ref m_CopiedColor, colorCopyDescriptor, name: "_FullscreenPassColorCopy");

            ScreenSpaceRelfectionsTex = RTHandles.Alloc("SSRT", name: "SSRT");
            downSample = ds;

            m_PassData ??= new PassData();
        }

        public void Dispose()
        {
            m_CopiedColor?.Release();
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.SetGlobalTexture("_ScreenSpaceRelfectionsTex", Shader.PropertyToID(ScreenSpaceRelfectionsTex.name));
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;

            RenderTextureDescriptor descriptor = cameraTargetDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 0;

            cameraTargetDescriptor = descriptor;
            //int downSample = 1;
            cameraTargetDescriptor.width /= downSample;
            cameraTargetDescriptor.height /= downSample;
            cameraTargetDescriptor.colorFormat = RenderTextureFormat.DefaultHDR;

            cmd.GetTemporaryRT(Shader.PropertyToID(ScreenSpaceRelfectionsTex.name), cameraTargetDescriptor, FilterMode.Bilinear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            m_PassData.effectMaterial = m_PassMaterial;
            m_PassData.passIndex = m_PassIndex;
            m_PassData.requiresColor = m_RequiresColor;
            m_PassData.isBeforeTransparents = m_IsBeforeTransparents;
            m_PassData.copiedColor = m_CopiedColor;

            ExecutePass(m_PassData, ref renderingData, ref context);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(ScreenSpaceRelfectionsTex.name));
        }

        // RG friendly method
        private void ExecutePass(PassData passData, ref RenderingData renderingData, ref ScriptableRenderContext context)
        {
            var passMaterial = passData.effectMaterial;
            var passIndex = passData.passIndex;
            var requiresColor = passData.requiresColor;
            var isBeforeTransparents = passData.isBeforeTransparents;
            var copiedColor = passData.copiedColor;

            if (passMaterial == null)
            {
                // should not happen as we check it in feature
                return;
            }

            if (renderingData.cameraData.isPreviewCamera)
            {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get();
            var cameraData = renderingData.cameraData;

            if (requiresColor)
            {
                // For some reason BlitCameraTexture(cmd, dest, dest) scenario (as with before transparents effects) blitter fails to correctly blit the data
                // Sometimes it copies only one effect out of two, sometimes second, sometimes data is invalid (as if sampling failed?).
                // Adding RTHandle in between solves this issue.
                var source = isBeforeTransparents ? cameraData.renderer.cameraColorTargetHandle : cameraData.renderer.cameraColorTargetHandle;

                Blitter.BlitCameraTexture(cmd, source, copiedColor);
                passMaterial.SetTexture(m_BlitTextureShaderID, copiedColor);
            }

            CoreUtils.SetRenderTarget(cmd, ScreenSpaceRelfectionsTex);
            CoreUtils.DrawFullScreen(cmd, passMaterial);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        private class PassData
        {
            internal Material effectMaterial;
            internal int passIndex;
            internal bool requiresColor;
            internal bool isBeforeTransparents;
            public RTHandle copiedColor;
        }
    }

    class Composite_Pass : ScriptableRenderPass
    {
        private Material m_PassMaterial;
        private int m_PassIndex;
        private bool m_RequiresColor;
        private bool m_IsBeforeTransparents;
        private PassData m_PassData;
        private RTHandle m_CopiedColor;
        private static readonly int m_BlitTextureShaderID = Shader.PropertyToID("_BlitTexture");

        public void Setup(Material mat, int index, bool requiresColor, bool isBeforeTransparents, string featureName, in RenderingData renderingData)
        {
            m_PassMaterial = mat;
            m_PassIndex = index;
            m_RequiresColor = requiresColor;
            m_IsBeforeTransparents = isBeforeTransparents;

            var colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
            RenderingUtils.ReAllocateIfNeeded(ref m_CopiedColor, colorCopyDescriptor, name: "_FullscreenPassColorCopy");

            m_PassData ??= new PassData();
        }

        public void Dispose()
        {
            m_CopiedColor?.Release();
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            m_PassData.effectMaterial = m_PassMaterial;
            m_PassData.passIndex = m_PassIndex;
            m_PassData.requiresColor = m_RequiresColor;
            m_PassData.isBeforeTransparents = m_IsBeforeTransparents;
            m_PassData.copiedColor = m_CopiedColor;

            ExecutePass(m_PassData, ref renderingData, ref context);
        }

        // RG friendly method
        private void ExecutePass(PassData passData, ref RenderingData renderingData, ref ScriptableRenderContext context)
        {
            var passMaterial = passData.effectMaterial;
            var passIndex = passData.passIndex;
            var requiresColor = passData.requiresColor;
            var isBeforeTransparents = passData.isBeforeTransparents;
            var copiedColor = passData.copiedColor;

            if (passMaterial == null)
            {
                // should not happen as we check it in feature
                return;
            }

            if (renderingData.cameraData.isPreviewCamera)
            {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get();
            var cameraData = renderingData.cameraData;

            if (requiresColor)
            {
                // For some reason BlitCameraTexture(cmd, dest, dest) scenario (as with before transparents effects) blitter fails to correctly blit the data
                // Sometimes it copies only one effect out of two, sometimes second, sometimes data is invalid (as if sampling failed?).
                // Adding RTHandle in between solves this issue.
                var source = isBeforeTransparents ? cameraData.renderer.cameraColorTargetHandle : cameraData.renderer.cameraColorTargetHandle;

                Blitter.BlitCameraTexture(cmd, source, copiedColor);
                passMaterial.SetTexture(m_BlitTextureShaderID, copiedColor);
            }

            CoreUtils.SetRenderTarget(cmd, cameraData.renderer.cameraColorTargetHandle);
            CoreUtils.DrawFullScreen(cmd, passMaterial);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        private class PassData
        {
            internal Material effectMaterial;
            internal int passIndex;
            internal bool requiresColor;
            internal bool isBeforeTransparents;
            public RTHandle copiedColor;
        }
    }

}
