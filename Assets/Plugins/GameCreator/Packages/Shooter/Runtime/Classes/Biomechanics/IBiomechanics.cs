using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.Shooter
{
    public interface IBiomechanics
    {
        void Enter(Character character, ShooterWeapon weapon, float enterDuration);
        void Exit(Character character, ShooterWeapon weapon, float exitDuration);
    }
}