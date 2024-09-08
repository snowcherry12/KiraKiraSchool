using System;

namespace GameCreator.Runtime.Quests
{
    [Flags]
    public enum InterestLayer
    {
        Layer1 = 0b00001,
        Layer2 = 0b00010,
        Layer3 = 0b00100,
        Layer4 = 0b01000,
        Layer5 = 0b10000,
    }

    public static class InterestLayers
    {
        public const InterestLayer Every = (InterestLayer) 0b11111;
        public const InterestLayer None = 0b00000;
    }
}