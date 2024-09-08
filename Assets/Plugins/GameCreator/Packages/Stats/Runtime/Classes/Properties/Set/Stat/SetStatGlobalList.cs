using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the Stat value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable]
    public class SetStatGlobalList : PropertyTypeSetStat
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueStat.TYPE_ID);

        public override void Set(Stat value, Args args) => this.m_Variable.Set(value, args);
        public override Stat Get(Args args) => this.m_Variable.Get(args) as Stat;

        public static PropertySetStat Create => new PropertySetStat(
            new SetStatGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}