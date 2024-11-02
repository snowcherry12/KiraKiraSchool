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
                "fullscreen",
                "Changes the fullscreen mode",
                value =>
                {
                    if (value == "true" || value == "false")
                    {
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, value == "true" ? true : false);
                        return Output.Success($"Graphic fullscreen = {value}");
                    }
                    else
                    {
                        return Output.Error($"Invalid value: {value}");
                    }
                }
            ),
        }) { }
    }
}