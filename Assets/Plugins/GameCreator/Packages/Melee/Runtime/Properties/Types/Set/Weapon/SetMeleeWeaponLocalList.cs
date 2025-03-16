using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
	[Title("Local List Variable")]
	[Category("Variables/Local List Variable")]

	[Description("Sets the Melee Weapon value of a Local List Variable")]
	[Image(typeof(IconListVariable), ColorTheme.Type.Purple)]

	[Serializable]
	public class SetMeleeWeaponLocalList : PropertyTypeSetWeapon
	{
		[SerializeField]
		protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueMeleeWeapon.TYPE_ID);

		public override void Set(IWeapon value, Args args) => this.m_Variable.Set(value, args);
		public override IWeapon Get(Args args) => this.m_Variable.Get(args) as IWeapon;

		public static PropertySetWeapon Create => new PropertySetWeapon(
			new SetMeleeWeaponLocalList()
		);

		public override string String => this.m_Variable.ToString();
	}
}