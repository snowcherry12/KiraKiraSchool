using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Quest value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestLocalList : PropertyTypeGetQuest
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueQuest.TYPE_ID);

        public override Quest Get(Args args) => this.m_Variable.Get<Quest>(args);

        public override string String => this.m_Variable.ToString();
    }
}