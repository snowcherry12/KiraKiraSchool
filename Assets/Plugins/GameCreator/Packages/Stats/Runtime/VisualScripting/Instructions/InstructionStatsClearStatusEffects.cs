using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Clear Status Effects Type")]
    [Category("Stats/Clear Status Effects Type")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green, typeof(OverlayCross))]
    [Description("Clears any Status Effects based on their type from the selected game object's Traits component")]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("Types", "The type of Status Effects that are cleared")]

    [Keywords("Buff", "Debuff", "Enhance", "Ailment")]
    [Keywords(
        "Blind", "Dark", "Burn", "Confuse", "Dizzy", "Stagger", "Fear", "Freeze", "Paralyze", 
        "Shock", "Silence", "Sleep", "Silence", "Slow", "Toad", "Weak", "Strong", "Poison"
    )]
    [Keywords(
        "Haste", "Protect", "Reflect", "Regenerate", "Shell", "Armor", "Shield", "Berserk",
        "Focus", "Raise"
    )]

    [Serializable]
    public class InstructionStatsClearStatusEffects : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private StatusEffectTypeMask m_Types = StatusEffectTypeMask.Negative;

        public override string Title => string.Format(
            "Clear {0} from {1}",
            (int) this.m_Types switch
            {
                 0 => "Nothing",
                -1 => "Everything",
                _ => this.m_Types.ToString()
            },
            this.m_Target
        );
        
        protected override Task Run(Args args)
        {
            GameObject target = this.m_Target.Get(args);
            if (target == null) return DefaultResult;

            Traits traits = target.Get<Traits>();
            if (traits == null) return DefaultResult;

            traits.RuntimeStatusEffects.ClearByType(this.m_Types);
            return DefaultResult;
        }
    }
}