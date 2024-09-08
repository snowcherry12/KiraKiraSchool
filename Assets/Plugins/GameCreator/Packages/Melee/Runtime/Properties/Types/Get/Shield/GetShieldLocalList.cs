using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Shield value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShieldLocalList : PropertyTypeGetShield
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueShield.TYPE_ID);

        public override IShield Get(Args args) => this.m_Variable.Get<Shield>(args);

        public override string String => this.m_Variable.ToString();
    }
}