using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]

    [Description("Sets the Skill value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable]
    public class SetSkillGlobalList : PropertyTypeSetSkill
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueSkill.TYPE_ID);

        public override void Set(Skill value, Args args) => this.m_Variable.Set(value, args);
        public override Skill Get(Args args) => this.m_Variable.Get(args) as Skill;

        public static PropertySetSkill Create => new PropertySetSkill(
            new SetSkillGlobalList()
        );

        public override string String => this.m_Variable.ToString();
    }
}
