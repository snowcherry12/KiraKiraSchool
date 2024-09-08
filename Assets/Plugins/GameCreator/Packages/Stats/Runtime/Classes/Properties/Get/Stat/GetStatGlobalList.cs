using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Stats
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Stat value of a Global List Variable")]

    [Serializable]
    public class GetStatGlobalList : PropertyTypeGetStat
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueStat.TYPE_ID);

        public override Stat Get(Args args) => this.m_Variable.Get<Stat>(args);

        public override string String => this.m_Variable.ToString();
    }
}