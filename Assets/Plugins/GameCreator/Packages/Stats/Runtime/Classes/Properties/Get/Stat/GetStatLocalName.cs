using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Stat value of a Local Name Variable")]
    
    [Serializable]
    public class GetStatLocalName : PropertyTypeGetStat
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueStat.TYPE_ID);

        public override Stat Get(Args args) => this.m_Variable.Get<Stat>(args);

        public override string String => this.m_Variable.ToString();
    }
}