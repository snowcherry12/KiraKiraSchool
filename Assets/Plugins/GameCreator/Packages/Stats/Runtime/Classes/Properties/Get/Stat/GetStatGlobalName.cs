using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Stat value of a Global Name Variable")]

    [Serializable]
    public class GetStatGlobalName : PropertyTypeGetStat
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueStat.TYPE_ID);

        public override Stat Get(Args args) => this.m_Variable.Get<Stat>(args);

        public override string String => this.m_Variable.ToString();
    }
}