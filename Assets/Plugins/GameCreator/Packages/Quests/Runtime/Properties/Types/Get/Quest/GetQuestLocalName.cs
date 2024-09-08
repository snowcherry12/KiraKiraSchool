using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Quest value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetQuestLocalName : PropertyTypeGetQuest
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueQuest.TYPE_ID);

        public override Quest Get(Args args) => this.m_Variable.Get<Quest>(args);

        public override string String => this.m_Variable.ToString();
    }
}