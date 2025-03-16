using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
	[Title("Local Name Variable")]
	[Category("Variables/Local Name Variable")]

	[Description("Sets the Melee Weapon value of a Local Name Variable")]
	[Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

	[Serializable]
	public class SetMeleeWeaponLocalName : PropertyTypeSetWeapon
	{
		[SerializeField]
		protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueMeleeWeapon.TYPE_ID);

		public override void Set(IWeapon value, Args args) => this.m_Variable.Set(value, args);
		public override IWeapon Get(Args args) => this.m_Variable.Get(args) as IWeapon;

		public static PropertySetWeapon Create => new PropertySetWeapon(
			new SetMeleeWeaponLocalName()
		);

		public override string String => this.m_Variable.ToString();
	}
}