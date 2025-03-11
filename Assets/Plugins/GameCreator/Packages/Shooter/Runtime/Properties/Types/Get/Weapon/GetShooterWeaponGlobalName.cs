using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shooter Global Name Variable")]
    [Category("Variables/Shooter Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Shooter Weapon value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShooterWeaponGlobalName : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueShooterWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<ShooterWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}