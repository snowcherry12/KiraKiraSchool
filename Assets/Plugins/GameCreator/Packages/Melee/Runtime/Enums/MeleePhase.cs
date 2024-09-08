namespace GameCreator.Runtime.Melee
{
    public enum MeleePhase
    {
        None         = 01, // 0b000001
        Reaction     = 02, // 0b000010
        Charge       = 04, // 0b000100
        Anticipation = 08, // 0b001000
        Strike       = 16, // 0b010000
        Recovery     = 32  // 0b100000
    }
}