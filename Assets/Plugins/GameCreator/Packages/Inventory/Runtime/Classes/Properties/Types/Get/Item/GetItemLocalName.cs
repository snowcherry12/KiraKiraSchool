using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Item value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetItemLocalName : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<Item>(args);

        public override string String => this.m_Variable.ToString();
    }
}