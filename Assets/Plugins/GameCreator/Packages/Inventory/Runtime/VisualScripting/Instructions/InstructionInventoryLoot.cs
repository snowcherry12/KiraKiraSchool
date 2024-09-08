using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Loot Table")]
    [Description("Picks a random choice from a Loot Table and sends it to the specified Bag")]

    [Category("Inventory/Loot/Loot Table")]
    
    [Parameter("Loot Table", "The Loot Table that generates the Item instance")]
    [Parameter("Bag", "The targeted Bag component")]

    [Keywords("Bag", "Inventory", "Container", "Stash")]
    [Keywords("Give", "Take", "Borrow", "Lend", "Corpse", "Generate")]
    
    [Image(typeof(IconLoot), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionInventoryLoot : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetLootTable m_LootTable = GetLootTableInstance.Create();
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Pick from {this.m_LootTable} to {this.m_Bag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (this.m_LootTable == null) return DefaultResult;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            LootTable lootTable = this.m_LootTable.Get(args);
            if (lootTable == null) return DefaultResult;
            
            lootTable.Run(bag);
            return DefaultResult;
        }
    }
}