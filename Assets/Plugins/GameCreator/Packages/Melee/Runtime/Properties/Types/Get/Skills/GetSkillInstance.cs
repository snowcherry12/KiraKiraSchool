using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Skill")]
    [Category("Skill")]
    
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.Green)]
    [Description("A reference to an Skill asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetSkillInstance : PropertyTypeGetSkill
    {
        [SerializeField] protected Skill m_Skill;

        public override Skill Get(Args args) => this.m_Skill;
        public override Skill Get(GameObject gameObject) => this.m_Skill;

        public static PropertyGetSkill Create(Skill skill = null)
        {
            GetSkillInstance instance = new GetSkillInstance
            {
                m_Skill = skill
            };
            
            return new PropertyGetSkill(instance);
        }

        public override string String => this.m_Skill != null
            ? this.m_Skill.name
            : "(none)";
    }
}