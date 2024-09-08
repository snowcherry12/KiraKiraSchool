namespace GameCreator.Runtime.Inventory
{
    public class LastLoot
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public LootTable LootTable { get; }
        
        public RuntimeItem RuntimeItem { get; }

        public Currency Currency { get; }

        public int Amount { get; }

        public bool IsItem => this.RuntimeItem != null;
        public bool IsCurrency => this.Currency != null;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public LastLoot(LootTable lootTable, RuntimeItem runtimeItem, int amount)
        {
            this.LootTable = lootTable;
            this.RuntimeItem = runtimeItem;
            this.Amount = amount;
        }
        
        public LastLoot(LootTable lootTable, Currency currency, int amount)
        {
            this.LootTable = lootTable;
            this.Currency = currency;
            this.Amount = amount;
        }
    }
}