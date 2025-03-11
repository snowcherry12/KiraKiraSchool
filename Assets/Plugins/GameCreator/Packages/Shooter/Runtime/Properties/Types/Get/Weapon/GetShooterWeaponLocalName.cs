using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
    [Title("Shooter Local Name Variable")]
    [Category("Variables/Shooter Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Shooter Weapon value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetShooterWeaponLocalName : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueShooterWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<ShooterWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}