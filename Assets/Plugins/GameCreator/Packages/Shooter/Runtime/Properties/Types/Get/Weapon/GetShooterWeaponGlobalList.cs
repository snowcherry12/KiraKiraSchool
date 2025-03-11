using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shooter Global List Variable")]
    [Category("Variables/Shooter Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Shooter Weapon value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShooterWeaponGlobalList : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueShooterWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<ShooterWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}