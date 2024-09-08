using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Shield value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShieldGlobalList : PropertyTypeGetShield
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueShield.TYPE_ID);

        public override IShield Get(Args args) => this.m_Variable.Get<Shield>(args);

        public override string String => this.m_Variable.ToString();
    }
}