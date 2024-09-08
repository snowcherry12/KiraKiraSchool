using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Local List Variable")]
    [Category("Local List Variable")]
    
    [Description("Sets the Quest value on a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetQuestLocalList : PropertyTypeSetQuest
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueQuest.TYPE_ID);

        public override void Set(Quest value, Args args) => this.m_Variable.Set(value, args);
        public override Quest Get(Args args) => this.m_Variable.Get(args) as Quest;

        public static PropertySetQuest Create => new PropertySetQuest(
            new SetQuestLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}