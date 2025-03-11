using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
	[Title("Local List Variable")]
	[Category("Variables/Local List Variable")]

	[Description("Sets the Shooter Weapon value of a Local List Variable")]
	[Image(typeof(IconListVariable), ColorTheme.Type.Purple)]

	[Serializable]
	public class SetShooterWeaponLocalList : PropertyTypeSetWeapon
	{
		[SerializeField]
		protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueShooterWeapon.TYPE_ID);

		public override void Set(IWeapon value, Args args) => this.m_Variable.Set(value, args);
		public override IWeapon Get(Args args) => this.m_Variable.Get(args) as IWeapon;

		public static PropertySetWeapon Create => new PropertySetWeapon(
			new SetShooterWeaponLocalList()
		);

		public override string String => this.m_Variable.ToString();
	}
}