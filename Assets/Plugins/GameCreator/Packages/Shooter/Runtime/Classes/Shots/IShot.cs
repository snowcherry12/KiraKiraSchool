using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Shooter
{
    public interface IShot
    {
        bool Run(
            Args args,
            ShooterWeapon weapon,
            MaterialSoundsAsset impactSound,
            PropertyGetInstantiate impactEffect,
            float chargeRatio,
            float pullTime
        );
    }
}