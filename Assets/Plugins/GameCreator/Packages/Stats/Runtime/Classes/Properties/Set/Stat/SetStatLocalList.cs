using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the Stat value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable]
    public class SetStatLocalList : PropertyTypeSetStat
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueStat.TYPE_ID);

        public override void Set(Stat value, Args args) => this.m_Variable.Set(value, args);
        public override Stat Get(Args args) => this.m_Variable.Get(args) as Stat;

        public static PropertySetStat Create => new PropertySetStat(
            new SetStatLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}