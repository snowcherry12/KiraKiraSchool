using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Set Skill")]
    [Description("Sets the Skill value")]

    [Category("Melee/Skills/Set Skill")]
    
    [Parameter("To", "The location where to store the Skill")]
    [Parameter("Skill", "The Skill asset reference")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionMeleeSetSkill : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertySetSkill m_To = SetSkillNone.Create;
        [SerializeField] private PropertyGetSkill m_Skill = new PropertyGetSkill();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_To} = {this.m_Skill}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Skill skill = this.m_Skill.Get(args);
            if (skill == null) return DefaultResult;

            this.m_To.Set(skill, args);
            return DefaultResult;
        }
    }
}