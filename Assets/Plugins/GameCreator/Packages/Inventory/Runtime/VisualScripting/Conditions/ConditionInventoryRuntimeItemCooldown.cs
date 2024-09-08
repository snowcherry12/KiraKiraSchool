using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Runtime Item Cooldown")]
    [Description("Returns true if the Bag's Runtime Item is currently on a cooldown state")]

    [Category("Inventory/Cooldowns/Is Runtime Item Cooldown")]
    
    [Parameter("Runtime Item", "The Runtime Item that checks its cooldown state")]

    [Keywords("Bag", "Cooldown", "Timer", "Timeout")]
    [Image(typeof(IconCooldown), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionInventoryRuntimeItemCooldown : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetRuntimeItem m_RuntimeItem = GetRuntimeItemLastUsed.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_RuntimeItem} in Cooldown";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Bag == null) return false;
            return !runtimeItem.Bag.Cooldowns.GetCooldown(runtimeItem.Item)?.IsReady ?? false;
        }
    }
}
