using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Shooter
{
	[Title("Global List Variable")]
	[Category("Variables/Global List Variable")]

	[Description("Sets the Shooter Weapon value of a Global List Variable")]
	[Image(typeof(IconListVariable), ColorTheme.Type.Purple)]

	[Serializable]
	public class SetShooterWeaponGlobalList : PropertyTypeSetWeapon
	{
		[SerializeField]
		protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueShooterWeapon.TYPE_ID);

		public override void Set(IWeapon value, Args args) => this.m_Variable.Set(value, args);
		public override IWeapon Get(Args args) => this.m_Variable.Get(args) as IWeapon;

		public static PropertySetWeapon Create => new PropertySetWeapon(
			new SetShooterWeaponGlobalList()
		);

		public override string String => this.m_Variable.ToString();
	}
}