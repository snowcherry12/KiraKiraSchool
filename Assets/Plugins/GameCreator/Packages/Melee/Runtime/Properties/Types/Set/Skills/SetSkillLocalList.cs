using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]

    [Description("Sets the Skill value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable]
    public class SetSkillLocalList : PropertyTypeSetSkill
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueSkill.TYPE_ID);

        public override void Set(Skill value, Args args) => this.m_Variable.Set(value, args);
        public override Skill Get(Args args) => this.m_Variable.Get(args) as Skill;

        public static PropertySetSkill Create => new PropertySetSkill(
            new SetSkillLocalList()
        );

        public override string String => this.m_Variable.ToString();
    }
}
