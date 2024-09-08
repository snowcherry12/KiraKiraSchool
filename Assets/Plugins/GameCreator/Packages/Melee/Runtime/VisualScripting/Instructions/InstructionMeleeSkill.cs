using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Play Melee Skill")]
    [Description("Plays a Skill on a Character regardless of the weapon or state")]

    [Category("Melee/Skills/Play Melee Skill")]
    
    [Parameter("Character", "The Character that plays the Skill")]
    [Parameter("Target", "Optional reference object set as the Target of the Skill")]
    [Parameter("Skill", "The Skill asset reference to run")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionMeleeSkill : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectNone.Create();

        [SerializeField] private PropertyGetWeapon m_Weapon = GetWeaponMeleeInstance.Create();
        [SerializeField] private PropertyGetSkill m_Skill = new PropertyGetSkill();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Skill {this.m_Skill} on {this.m_Character}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;

            GameObject target = this.m_Target.Get(args);
            
            Skill skill = this.m_Skill.Get(args);
            if (skill == null) return DefaultResult;

            MeleeWeapon weapon = this.m_Weapon.Get(args) as MeleeWeapon;

            character.Combat
                .RequestStance<MeleeStance>()
                .PlaySkill(weapon, skill, target);
            
            return DefaultResult;
        }
    }
}