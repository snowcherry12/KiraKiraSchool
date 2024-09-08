using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.Melee
{
    public enum MeleeDirection
    {
        None = ReactionDirection.FromAny,
        Left = ReactionDirection.FromLeft,
        Right = ReactionDirection.FromRight,
        Forward = ReactionDirection.FromFront,
        Backwards = ReactionDirection.FromBack,
        Upwards = ReactionDirection.FromTop,
        Downwards = ReactionDirection.FromBottom
    }
}