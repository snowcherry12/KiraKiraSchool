using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
	[Title("Local Name Variable")]
	[Category("Variables/Local Name Variable")]

	[Description("Sets the Shooter Weapon value of a Local Name Variable")]
	[Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

	[Serializable]
	public class SetShooterWeaponLocalName : PropertyTypeSetWeapon
	{
		[SerializeField]
		protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueShooterWeapon.TYPE_ID);

		public override void Set(IWeapon value, Args args) => this.m_Variable.Set(value, args);
		public override IWeapon Get(Args args) => this.m_Variable.Get(args) as IWeapon;

		public static PropertySetWeapon Create => new PropertySetWeapon(
			new SetShooterWeaponLocalName()
		);

		public override string String => this.m_Variable.ToString();
	}
}