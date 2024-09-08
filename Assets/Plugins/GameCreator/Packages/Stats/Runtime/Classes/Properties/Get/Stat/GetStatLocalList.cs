using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Stat value of a Local List Variable")]

    [Serializable]
    public class GetStatLocalList : PropertyTypeGetStat
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueStat.TYPE_ID);

        public override Stat Get(Args args) => this.m_Variable.Get<Stat>(args);

        public override string String => this.m_Variable.ToString();
    }
}