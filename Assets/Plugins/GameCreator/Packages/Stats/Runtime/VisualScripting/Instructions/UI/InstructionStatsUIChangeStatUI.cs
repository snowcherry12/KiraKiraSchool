using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [Version(0, 1, 1)]
    
    [Title("Change StatUI Target")]
    [Category("Stats/UI/Change StatUI Target")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Changes the targeted game object of an Stat UI component")]

    [Parameter("Stat UI", "The game object with the Stat UI component")]
    [Parameter("Target", "The new targeted game object with a Traits component")]

    [Serializable]
    public class InstructionStatsUIChangeStatUI : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_StatUI = GetGameObjectInstance.Create();

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        public override string Title => $"Change {this.m_StatUI} target to {this.m_Target}";
        
        protected override Task Run(Args args)
        {
            GameObject statUIGameObject = this.m_StatUI.Get(args);
            if (statUIGameObject == null) return DefaultResult;

            StatUI statUI = statUIGameObject.Get<StatUI>();
            if (statUI == null) return DefaultResult;

            statUI.Target = this.m_Target.Get(args);
            return DefaultResult;
        }
    }
}