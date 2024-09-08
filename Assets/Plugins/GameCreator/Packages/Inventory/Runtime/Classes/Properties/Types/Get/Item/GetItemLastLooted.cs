using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Looted")]
    [Category("Bags/Last Looted")]
    
    [Image(typeof(IconLoot), ColorTheme.Type.Red)]
    [Description("A reference to the last Item picked from a Loot Table")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastLooted : PropertyTypeGetItem
    {
        public override Item Get(Args args) => LootTable.LastLooted.IsItem
            ? LootTable.LastLooted.RuntimeItem?.Item
            : null;
        
        public override Item Get(GameObject gameObject) => LootTable.LastLooted.IsItem
            ? LootTable.LastLooted.RuntimeItem?.Item
            : null;

        public static PropertyGetItem Create()
        {
            GetItemLastLooted instance = new GetItemLastLooted();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Looted]";
    }
}