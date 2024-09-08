using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Add Runtime Item Cooldown")]
    [Description("Adds a cooldown timer for a Runtime Item's Bag")]

    [Category("Inventory/Cooldowns/Add Runtime Item Cooldown")]
    
    [Parameter("Runtime Item", "The Runtime Item instance to add a cooldown")]
    [Keywords("Bag", "Cooldown", "Timer", "Timeout")]
    
    [Image(typeof(IconCooldown), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionInventoryCooldownAddRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetRuntimeItem m_RuntimeItem = GetRuntimeItemLastUsed.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Add {this.m_RuntimeItem} Cooldown";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Bag) return DefaultResult;
            
            runtimeItem.Bag.Cooldowns.SetCooldown(runtimeItem.Item, args);
            return DefaultResult;
        }
    }
}