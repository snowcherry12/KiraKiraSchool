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
    public class GetRuntimeItemLastLooted : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => LootTable.LastLooted.IsItem
            ? LootTable.LastLooted.RuntimeItem
            : null;
        
        public override RuntimeItem Get(GameObject gameObject) => LootTable.LastLooted.IsItem
            ? LootTable.LastLooted.RuntimeItem
            : null;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastLooted instance = new GetRuntimeItemLastLooted();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Looted]";
    }
}