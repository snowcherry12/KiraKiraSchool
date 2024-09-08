using System;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Flags]
    public enum CellCorner
    {
        TopLeft     = 1, // 0x0001
        TopRight    = 2, // 0x0010
        BottomLeft  = 4, // 0x0100
        BottomRight = 8, // 0x1000
    }
}