using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global List Variable from Runtime Item")]
    [Category("Variables/Global List Variable from Runtime Item")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Item value of a Global List Variable with a Runtime Item")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemGlobalListFromRuntimeItem : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueRuntimeItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<RuntimeItem>(args)?.Item;

        public override string String => this.m_Variable.ToString();
    }
}