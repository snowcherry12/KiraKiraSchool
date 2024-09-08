using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Item value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemGlobalList : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<Item>(args);

        public override string String => this.m_Variable.ToString();
    }
}