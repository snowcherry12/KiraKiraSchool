using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Prop Shot")]
    [Category("Shooter/Last Prop Shot")]
    
    [Image(typeof(IconPistol), ColorTheme.Type.Yellow)]
    [Description("The game object Prop of the last Shooter Weapon that took a shot")]

    [Serializable]
    public class GetGameObjectShooterLastPropShot : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args) => ShotData.LastProp;
        public override GameObject Get(GameObject gameObject) => ShotData.LastProp;

        public override string String => "Last Prop Shot";
    }
}