using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Weapon Shot")]
    [Category("Shooter/Shots/Last Weapon Shot")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Green)]
    [Description("A reference to the last Shooter Weapon asset used by any character")]

    [Serializable]
    public class GetWeaponShooterLastShot : PropertyTypeGetWeapon
    {
        public override IWeapon Get(Args args) => ShotData.LastShooterWeapon;
        public override IWeapon Get(GameObject gameObject) => ShotData.LastShooterWeapon;

        public override string String => "Last Weapon Shot";
    }
}