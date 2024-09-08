using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the Stat value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable]
    public class SetStatLocalName : PropertyTypeSetStat
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueStat.TYPE_ID);

        public override void Set(Stat value, Args args) => this.m_Variable.Set(value, args);
        public override Stat Get(Args args) => this.m_Variable.Get(args) as Stat;

        public static PropertySetStat Create => new PropertySetStat(
            new SetStatLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}