using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Item Cooldown")]
    [Description("Returns true if the Bag's Item is currently on a cooldown state")]

    [Category("Inventory/Cooldowns/Is Item Cooldown")]
    
    [Parameter("Bag", "The Bag targeted")]
    [Parameter("Item", "The Item that checks its cooldown state")]

    [Keywords("Bag", "Cooldown", "Timer", "Timeout")]
    [Image(typeof(IconCooldown), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionInventoryItemCooldown : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetItem m_Item = GetItemInstance.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Item} in Cooldown on {this.m_Bag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return false;

            Item item = this.m_Item.Get(args);
            if (item == null) return false;

            return !bag.Cooldowns.GetCooldown(item)?.IsReady ?? false;
        }
    }
}
