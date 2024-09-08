using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Set Status Effect")]
    [Category("Stats/Set Status Effect")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Sets a Status Effect value")]

    [Parameter("To", "Where to store the Status Effect asset")]
    [Parameter("Status Effect", "The Status Effect asset to store")]

    [Serializable]
    public class InstructionStatusEffectsSetStatusEffect : Instruction
    {
        [SerializeField] private PropertySetStatusEffect m_To = SetStatusEffectNone.Create;
        [SerializeField] private PropertyGetStatusEffect m_StatusEffect = new PropertyGetStatusEffect();

        public override string Title => $"Set {this.m_To} = {this.m_StatusEffect}";
        
        protected override Task Run(Args args)
        {
            this.m_To.Set(this.m_StatusEffect.Get(args), args);
            return DefaultResult;
        }
    }
}