using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Quest value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestGlobalName : PropertyTypeGetQuest
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueQuest.TYPE_ID);

        public override Quest Get(Args args) => this.m_Variable.Get<Quest>(args);

        public override string String => this.m_Variable.ToString();
    }
}