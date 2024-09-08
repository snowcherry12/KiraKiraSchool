using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Quests
{
    [Title("Global Name Variable")]
    [Category("Global Name Variable")]
    
    [Description("Sets the Quest value on a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetQuestGlobalName : PropertyTypeSetQuest
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueQuest.TYPE_ID);

        public override void Set(Quest value, Args args) => this.m_Variable.Set(value, args);
        public override Quest Get(Args args) => this.m_Variable.Get(args) as Quest;

        public static PropertySetQuest Create => new PropertySetQuest(
            new SetQuestGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}