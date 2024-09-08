using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [Version(0, 1, 1)]
    
    [Title("Change Status Effects List UI Target")]
    [Category("Stats/UI/Change Status Effects List UI Target")]
    
    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]
    [Description("Changes the targeted game object of an Status Effects List UI component")]

    [Parameter("Status Effects List UI", "The game object with the Status Effects List UI component")]
    [Parameter("Target", "The new targeted game object with a Traits component")]

    [Serializable]
    public class InstructionStatsUIChangeStatusEffectsListUI : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_StatusEffectsListUI = GetGameObjectInstance.Create();

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        public override string Title => $"Change {this.m_StatusEffectsListUI} target to {this.m_Target}";
        
        protected override Task Run(Args args)
        {
            GameObject statusEffectsListUIGameObject = this.m_StatusEffectsListUI.Get(args);
            if (statusEffectsListUIGameObject == null) return DefaultResult;

            StatusEffectListUI statUI = statusEffectsListUIGameObject.Get<StatusEffectListUI>();
            if (statUI == null) return DefaultResult;

            statUI.Target = this.m_Target.Get(args);
            return DefaultResult;
        }
    }
}