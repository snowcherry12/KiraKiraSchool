using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Melee Global List Variable")]
    [Category("Variables/Melee Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Melee Weapon value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetMeleeWeaponGlobalList : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueMeleeWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<MeleeWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}