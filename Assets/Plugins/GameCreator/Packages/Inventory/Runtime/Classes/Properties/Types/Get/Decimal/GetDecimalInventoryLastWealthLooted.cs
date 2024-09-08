using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Wealth Looted")]
    [Category("Inventory/Last Wealth Looted")]
    
    [Image(typeof(IconLoot), ColorTheme.Type.Red)]
    [Description("The last amount of a currency picked from running a Loot Table")]

    [Serializable]
    public class GetDecimalInventoryLastWealthLooted : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => LootTable.LastLooted.IsCurrency 
            ? LootTable.LastLooted.Amount 
            : 0;

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryLastWealthLooted()
        );

        public override string String => "Wealth[Last Looted]";
    }
}