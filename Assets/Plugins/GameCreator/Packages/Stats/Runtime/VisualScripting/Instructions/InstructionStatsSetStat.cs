using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Set Stat")]
    [Category("Stats/Set Stat")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Sets a Stat value")]

    [Parameter("To", "Where to store the Stat asset")]
    [Parameter("Stat", "The Stat asset to store")]

    [Serializable]
    public class InstructionStatsSetStat : Instruction
    {
        [SerializeField] private PropertySetStat m_To = SetStatNone.Create;
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        public override string Title => $"Set {this.m_To} = {this.m_Stat}";
        
        protected override Task Run(Args args)
        {
            this.m_To.Set(this.m_Stat.Get(args), args);
            return DefaultResult;
        }
    }
}