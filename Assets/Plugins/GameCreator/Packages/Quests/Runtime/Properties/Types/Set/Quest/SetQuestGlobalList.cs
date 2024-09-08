using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Global List Variable")]
    [Category("Global List Variable")]
    
    [Description("Sets the Quest value on a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetQuestGlobalList : PropertyTypeSetQuest
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueQuest.TYPE_ID);

        public override void Set(Quest value, Args args) => this.m_Variable.Set(value, args);
        public override Quest Get(Args args) => this.m_Variable.Get(args) as Quest;

        public static PropertySetQuest Create => new PropertySetQuest(
            new SetQuestGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}