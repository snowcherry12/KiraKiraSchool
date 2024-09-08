using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Stat value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable]
    public class SetStatGlobalName : PropertyTypeSetStat
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueStat.TYPE_ID);

        public override void Set(Stat value, Args args) => this.m_Variable.Set(value, args);
        public override Stat Get(Args args) => this.m_Variable.Get(args) as Stat;

        public static PropertySetStat Create => new PropertySetStat(
            new SetStatGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}