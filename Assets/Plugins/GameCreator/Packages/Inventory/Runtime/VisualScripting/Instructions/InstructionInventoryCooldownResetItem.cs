using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Reset Item Cooldown")]
    [Description("Removes the cooldown timer of a Bag's Item")]

    [Category("Inventory/Cooldowns/Reset Item Cooldown")]
    
    [Parameter("Bag", "The Bag where the Item belongs to")]
    [Parameter("Item", "The Item asset to reset its cooldown")]

    [Keywords("Bag", "Cooldown", "Timer", "Timeout")]
    
    [Image(typeof(IconCooldown), ColorTheme.Type.Green, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionInventoryCooldownResetItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetItem m_Item = GetItemInstance.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Reset {this.m_Item} Cooldown on {this.m_Bag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            Item item = this.m_Item.Get(args);
            if (item == null) return DefaultResult;
            
            bag.Cooldowns.ResetCooldown(item);
            return DefaultResult;
        }
    }
}