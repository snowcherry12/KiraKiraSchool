using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]

    [Description("Sets the Skill value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable]
    public class SetSkillGlobalName : PropertyTypeSetSkill
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueSkill.TYPE_ID);

        public override void Set(Skill value, Args args) => this.m_Variable.Set(value, args);
        public override Skill Get(Args args) => this.m_Variable.Get(args) as Skill;

        public static PropertySetSkill Create => new PropertySetSkill(
            new SetSkillGlobalName()
        );

        public override string String => this.m_Variable.ToString();
    }
}
