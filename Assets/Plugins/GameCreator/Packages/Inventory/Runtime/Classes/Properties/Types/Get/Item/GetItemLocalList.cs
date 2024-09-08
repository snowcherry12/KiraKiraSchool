using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Item value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLocalList : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<Item>(args);

        public override string String => this.m_Variable.ToString();
    }
}