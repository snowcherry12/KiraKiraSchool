using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Last Shot Hit")]
    [Category("Shooter/Last Shot Hit")]
    
    [Image(typeof(IconShoot), ColorTheme.Type.Yellow)]
    [Description("The impact position of the last shot taken")]

    [Serializable]
    public class GetPositionShooterLastHitPosition : PropertyTypeGetPosition
    {
        public override Vector3 Get(Args args) => ShotData.LastShooterPosition;
        public override Vector3 Get(GameObject gameObject) => ShotData.LastShooterPosition;

        public override string String => "Last Shot Hit";
    }
}