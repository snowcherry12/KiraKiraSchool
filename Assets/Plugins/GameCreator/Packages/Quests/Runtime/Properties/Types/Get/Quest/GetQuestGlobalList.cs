using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Quest value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestGlobalList : PropertyTypeGetQuest
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueQuest.TYPE_ID);

        public override Quest Get(Args args) => this.m_Variable.Get<Quest>(args);

        public override string String => this.m_Variable.ToString();
    }
}