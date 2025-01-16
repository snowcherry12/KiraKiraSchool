using System;

namespace UnityEngine.Rendering.Universal
{
    [Serializable, VolumeComponentMenuForRenderPipeline("GapperGames/Screen Space Reflections", typeof(UniversalRenderPipeline))]
    public sealed partial class ScreenSpaceReflections : VolumeComponent, IPostProcessComponent
    {
        public BoolParameter enabled = new BoolParameter(false);
        public ClampedIntParameter downsample = new ClampedIntParameter(2, 1, 8);
        public MinIntParameter samples = new MinIntParameter(8, 0);
        public MinIntParameter steps = new MinIntParameter(125, 0);
        public ClampedFloatParameter stepSize = new ClampedFloatParameter(0.1f, 0f, 10f);
        public ClampedFloatParameter thickness = new ClampedFloatParameter(0.1f, 0f, 10f);
        public ClampedFloatParameter minSmoothness = new ClampedFloatParameter(0.1f, 0, 1);

        /// <inheritdoc/>
        public bool IsActive() => enabled.value;

        /// <inheritdoc/>
        public bool IsTileCompatible() => false;
    }
}
