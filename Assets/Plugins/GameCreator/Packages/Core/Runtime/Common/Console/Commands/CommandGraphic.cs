using System;
using GameCreator.Runtime.Common;
using UnityEngine;

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
                "fullscreen",
                "Changes the fullscreen mode",
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
                "Changes the resolution",
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
                "Changes the game quality",
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
                "Changes the render scale",
                value =>
                {
                    float scale = Convert.ToSingle(value);
                    QualitySettings.resolutionScalingFixedDPIFactor = scale;
                    return Output.Success($"Graphic render scale = {QualitySettings.resolutionScalingFixedDPIFactor}");
                }
            ),
            new ActionOutput(
                "fsr",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic FSR = {value}");
                }
            ),
            new ActionOutput(
                "shadow",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic display scale = {value}");
                }
            ),
            new ActionOutput(
                "texture-quality",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic display scale = {value}");
                }
            ),
            new ActionOutput(
                "anisotropic-filtering",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic display scale = {value}");
                }
            ),
            new ActionOutput(
                "anti-aliasing",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic display scale = {value}");
                }
            ),
            new ActionOutput(
                "vsync",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic display scale = {value}");
                }
            ),
            new ActionOutput(
                "fps",
                "Changes the FSR scale",
                value =>
                {
                    return Output.Success($"Graphic display scale = {value}");
                }
            ),
        }) { }
    }
}