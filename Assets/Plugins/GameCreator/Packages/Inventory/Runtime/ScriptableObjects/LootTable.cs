using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Loot Table",
        menuName = "Game Creator/Inventory/Loot Table"
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoLootTable.png")]
    [Serializable]
    public class LootTable : ScriptableObject
    {
        public static LastLoot LastLooted   { get; private set; }

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private int m_NoDropRate;
        [SerializeField] private LootList m_LootList = new LootList();

        // PROPERTIES: ----------------------------------------------------------------------------

        public int NoDropRate => this.m_NoDropRate;

        public Loot[] Loot => this.m_LootList.List;

        public int TotalRate
        {
            get
            {
                int rate = this.m_NoDropRate;
                foreach (Loot loot in this.m_LootList.List)
                {
                    rate += loot.Rate;
                }

                return rate;
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public static event Action<LootTable, Bag> EventLoot;
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventLoot = null;
        }
        
        #endif
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Run(Bag outputBag)
        {
            List<Loot> chances = new List<Loot>();
            int totalRate = 0;

            if (this.m_NoDropRate > 0)
            {
                totalRate += this.m_NoDropRate;
                chances.Add(new Loot(null, 0, this.m_NoDropRate));
            }

            foreach (Loot loot in this.m_LootList.List)
            {
                chances.Add(loot);
                totalRate += loot.Rate;
            }

            chances.Sort((x, y) => y.Rate.CompareTo(x.Rate));
            int random = UnityEngine.Random.Range(0, totalRate);

            foreach (Loot loot in chances)
            {
                if (random < loot.Rate)
                {
                    int amount = loot.Amount;

                    if (loot.IsItem)
                    {
                        Item item = loot.Item;
                        if (item == null) return false;
                        
                        bool atLeastOne = false;
                        
                        for (int i = 0; i < amount; ++i)
                        {
                            RuntimeItem runtimeItem = outputBag.Content.AddType(item, true);
                            if (runtimeItem == null) continue;

                            LastLooted = new LastLoot(this, runtimeItem, amount);
                            EventLoot?.Invoke(this, outputBag);
                        
                            atLeastOne = true;
                        }
                    
                        return atLeastOne;
                    }
                    
                    if (loot.IsCurrency)
                    {
                        Currency currency = loot.Currency;
                        if (currency == null) return false;
                        
                        outputBag.Wealth.Add(currency, amount);

                        LastLooted = new LastLoot(this, currency, amount);
                        EventLoot?.Invoke(this, outputBag);

                        return true;
                    }
                }

                random -= loot.Rate;
            }

            return false;
        }
    }
}