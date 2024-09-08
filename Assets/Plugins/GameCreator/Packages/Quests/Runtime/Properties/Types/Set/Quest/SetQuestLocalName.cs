using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Local Name Variable")]
    [Category("Local Name Variable")]
    
    [Description("Sets the Quest value on a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetQuestLocalName : PropertyTypeSetQuest
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueQuest.TYPE_ID);

        public override void Set(Quest value, Args args) => this.m_Variable.Set(value, args);
        public override Quest Get(Args args) => this.m_Variable.Get(args) as Quest;

        public static PropertySetQuest Create => new PropertySetQuest(
            new SetQuestLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}