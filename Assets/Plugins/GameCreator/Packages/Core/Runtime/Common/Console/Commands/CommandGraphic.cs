using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GameCreator.Runtime.Console
{
    public sealed class CommandGraphic : Command
    {        
        public override string Name => "graphic";

        public override string Description => "Changes the graphic settings of the game";

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public CommandGraphic() : base(new[]
        {
            new ActionOutput(
                "brightness",
                "Changes the Screen Brightness",
                value =>
                {
                    
                    Screen.brightness = Convert.ToSingle(value);
                    return Output.Success($"Graphic Brightness = {Screen.brightness}");
                }
            ),
            new ActionOutput(
                "contrast",
                "Changes the Screen Contrast",
                value =>
                {
                    
                    return Output.Success($"Graphic Contrast = {value}");
                }
            ),
            new ActionOutput(
                "fullscreen",
                "Changes the Fullscreen Mode",
                value =>
                {
                    switch (value)
                    {
                        case "True":
                            Screen.fullScreen = true;
                            break;
                        case "False":
                            Screen.fullScreen = false;
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic fullscreen = {Screen.fullScreen}");
                }
            ),
            new ActionOutput(
                "resolution",
                "Changes the Resolution",
                value =>
                {
                    string[] resolution = value.Split("x");
                    int width = Convert.ToInt32(resolution[0]);
                    int height = Convert.ToInt32(resolution[1]);
                    Screen.SetResolution(width, height, Screen.fullScreen);
                    return Output.Success($"Graphic resolution = {value}");
                }
            ),
            new ActionOutput(
                "quality",
                "Changes the Quality Level",
                value =>
                {
                    switch (value)
                    {
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                            QualitySettings.SetQualityLevel(Convert.ToInt32(value), true);
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic quality = {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
                }
            ),
            new ActionOutput(
                "render-scale",
                "Changes the Render Scale",
                value =>
                {
                    float scale = Convert.ToSingle(value);
                    QualitySettings.resolutionScalingFixedDPIFactor = scale;
                    return Output.Success($"Graphic render scale = {QualitySettings.resolutionScalingFixedDPIFactor}");
                }
            ),
            new ActionOutput(
                "fsr",
                "Changes the FSR Scale",
                value =>
                {
                    UniversalRenderPipelineAsset data = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                    data.fsrSharpness = Convert.ToSingle(value);
                    return Output.Success($"Graphic FSR = {data.fsrSharpness}");
                }
            ),
            new ActionOutput(
                "shadow",
                "Changes the Shadow Quality",
                value =>
                {
                    UniversalRenderPipelineAsset data = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                    switch (value)
                    {
                        case "0":
                            QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
                            break;
                        case "1":
                            QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
                            QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low;
                            break;
                        case "2":
                            QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                            QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Medium;
                            break;
                        case "3":
                            QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                            QualitySettings.shadowResolution = UnityEngine.ShadowResolution.High;
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic shadow = {value}");
                }
            ),
            new ActionOutput(
                "texture-quality",
                "Changes the Texture Quality",
                value =>
                {
                    QualitySettings.globalTextureMipmapLimit = Convert.ToInt32(value);
                    return Output.Success($"Graphic texture quality = {value}");
                }
            ),
            new ActionOutput(
                "anisotropic-filtering",
                "Changes the AnisotropicFiltering",
                value =>
                {
                    // UniversalRenderPipelineAsset data = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                    switch (value)
                    {
                        case "0":
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic anisotropic filtering = {value}");
                }
            ),
            new ActionOutput(
                "anti-aliasing",
                "Changes the AntiAliasing",
                value =>
                {
                    switch (value)
                    {
                        case "0":
                            QualitySettings.antiAliasing = 0;
                            break;
                        case "1":
                            QualitySettings.antiAliasing = 2;
                            break;
                        case "2":
                            QualitySettings.antiAliasing = 4;
                            break;
                        case "3":
                            QualitySettings.antiAliasing = 8;
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic antialiasing = {value}");
                }
            ),
            new ActionOutput(
                "vsync",
                "Changes the vSync",
                value =>
                {
                    switch (value)
                    {
                        case "True":
                            QualitySettings.vSyncCount = 1;
                            break;
                        case "False":
                            QualitySettings.vSyncCount = 0;
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic vsync = {value}");
                }
            ),
            new ActionOutput(
                "fps",
                "Changes the FPS",
                value =>
                {
                    switch (value)
                    {
                        case "0":
                            Application.targetFrameRate = -1;
                            break;
                        case "1":
                            Application.targetFrameRate = 240;
                            break;
                        case "2":
                            Application.targetFrameRate = 120;
                            break;
                        case "3":
                            Application.targetFrameRate = 60;
                            break;
                        case "4":
                            Application.targetFrameRate = 30;
                            break;
                        default:
                            return Output.Error($"Invalid value: {value}");
                    }
                    return Output.Success($"Graphic fps = {Application.targetFrameRate}");
                }
            ),
        }) { }
    }
}