using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Item value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemGlobalName : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<Item>(args);

        public override string String => this.m_Variable.ToString();
    }
}