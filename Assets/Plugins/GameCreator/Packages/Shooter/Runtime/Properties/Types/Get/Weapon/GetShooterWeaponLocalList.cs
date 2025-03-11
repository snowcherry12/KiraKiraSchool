using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shooter Local List Variable")]
    [Category("Variables/Shooter Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Shooter Weapon value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShooterWeaponLocalList : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueShooterWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<ShooterWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}