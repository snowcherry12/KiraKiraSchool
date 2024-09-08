using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Reset Runtime Item Cooldown")]
    [Description("Removes the cooldown timer of the Runtime Item's Bag")]

    [Category("Inventory/Cooldowns/Reset Runtime Item Cooldown")]
    
    [Parameter("Item", "The Runtime Item instance to reset its cooldown")]

    [Keywords("Bag", "Cooldown", "Timer", "Timeout")]
    
    [Image(typeof(IconCooldown), ColorTheme.Type.Blue, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionInventoryCooldownResetRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetRuntimeItem m_RuntimeItem = GetRuntimeItemLastUsed.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Reset {this.m_RuntimeItem} Cooldown";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Bag) return DefaultResult;
            
            runtimeItem.Bag.Cooldowns.ResetCooldown(runtimeItem.Item);
            return DefaultResult;
        }
    }
}