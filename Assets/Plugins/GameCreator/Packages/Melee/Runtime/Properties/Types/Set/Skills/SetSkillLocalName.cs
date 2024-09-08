using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Description("Sets the Skill value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable]
    public class SetSkillLocalName : PropertyTypeSetSkill
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueSkill.TYPE_ID);

        public override void Set(Skill value, Args args) => this.m_Variable.Set(value, args);
        public override Skill Get(Args args) => this.m_Variable.Get(args) as Skill;

        public static PropertySetSkill Create => new PropertySetSkill(
            new SetSkillLocalName()
        );

        public override string String => this.m_Variable.ToString();
    }
}
