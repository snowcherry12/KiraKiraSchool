using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local Name Variable")]
    [Category("Local Name Variable")]
    
    [Description("Sets the Item value on a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetItemLocalName : PropertyTypeSetItem
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueItem.TYPE_ID);

        public override void Set(Item value, Args args) => this.m_Variable.Set(value, args);
        public override Item Get(Args args) => this.m_Variable.Get(args) as Item;

        public static PropertySetItem Create => new PropertySetItem(
            new SetItemLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}