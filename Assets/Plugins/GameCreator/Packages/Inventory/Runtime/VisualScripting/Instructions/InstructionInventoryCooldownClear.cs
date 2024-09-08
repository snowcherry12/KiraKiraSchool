using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Clear Cooldowns")]
    [Description("Removes all cooldowns on a Bag")]

    [Category("Inventory/Cooldowns/Clear Cooldowns")]
    
    [Parameter("Bag", "The Bag where all cooldowns are removed from")]

    [Keywords("Bag", "Cooldown", "Timer", "Timeout")]
    
    [Image(typeof(IconCooldown), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionInventoryCooldownClear : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Clear {this.m_Bag} Cooldowns";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            bag.Cooldowns.ClearCooldowns();
            return DefaultResult;
        }
    }
}