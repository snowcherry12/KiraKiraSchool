using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class RuntimeItem
    {
        public static RuntimeItem Bag_LastItemAttemptedUse     { get; internal set; }
        public static RuntimeItem Bag_LastItemAttemptedAdd     { get; internal set; }
        public static RuntimeItem Bag_LastItemAttemptedRemove  { get; internal set; }

        public static RuntimeItem Bag_LastItemUsed    { get; internal set; }
        public static RuntimeItem Bag_LastItemAdded   { get; internal set; }
        public static RuntimeItem Bag_LastItemRemoved { get; internal set; }

        public static RuntimeItem Bag_LastItemEquipped { get; internal set; }
        public static RuntimeItem Bag_LastItemUnequipped { get; internal set; }

        public static RuntimeItem Socket_LastParentAttached { get; internal set; }
        public static RuntimeItem Socket_LastParentDetached { get; internal set; }
        public static RuntimeItem Socket_LastAttachmentAttached { get; internal set; }
        public static RuntimeItem Socket_LastAttachmentDetached { get; internal set; }

        public static RuntimeItem UI_LastItemHovered { get; internal set; }
        public static RuntimeItem UI_LastItemSelected { get; internal set; }
        public static RuntimeSocket UI_LastSocketHovered { get; internal set; }
        public static RuntimeSocket UI_LastSocketSelected { get; internal set; }
        public static RuntimeProperty UI_LastPropertyHovered { get; internal set; }
        public static RuntimeProperty UI_LastPropertySelected { get; internal set; }
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            Bag_LastItemAttemptedUse = null;
            Bag_LastItemAttemptedAdd = null;
            Bag_LastItemAttemptedRemove = null;

            Bag_LastItemUsed = null;
            Bag_LastItemAdded = null;
            Bag_LastItemRemoved = null;

            Bag_LastItemEquipped = null;
            Bag_LastItemUnequipped = null;

            Socket_LastParentAttached = null;
            Socket_LastParentDetached = null;
            Socket_LastAttachmentAttached = null;
            Socket_LastAttachmentDetached = null;
            
            UI_LastItemHovered = null;
            UI_LastItemSelected = null;
            UI_LastSocketHovered = null;
            UI_LastSocketSelected = null;
            UI_LastPropertyHovered = null;
            UI_LastPropertySelected = null;
        }
        
        #endif
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private IdString m_ItemID;
        [SerializeField] private IdString m_RuntimeID;

        [SerializeField] private RuntimeProperties m_Properties;
        [SerializeField] private RuntimeSockets m_Sockets;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Item m_Item;
        [NonSerialized] private Bag m_Bag;

        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString ItemID => this.m_ItemID;
        public IdString RuntimeID => this.m_RuntimeID;

        public RuntimeProperties Properties => this.m_Properties;
        public RuntimeSockets Sockets => this.m_Sockets;

        public Bag Bag
        {
            get => this.m_Bag;
            set
            {
                this.m_Bag = value;
                foreach (KeyValuePair<IdString, RuntimeSocket> entry in this.m_Sockets)
                {
                    if (!entry.Value.HasAttachment) continue;
                    entry.Value.Attachment.Bag = value;
                }
            }
        }

        public Item Item
        {
            get
            {
                if (this.m_Item == null)
                {
                    InventoryRepository inventory = Settings.From<InventoryRepository>();
                    this.m_Item = inventory.Items.Get(this.m_ItemID);
                }

                return this.m_Item;
            }
        }

        public int Weight
        {
            get
            {
                int weight = this.Item.Shape.Weight;
                if (this.Bag == null) return weight;
                
                foreach (KeyValuePair<IdString, RuntimeSocket> entry in this.m_Sockets)
                {
                    if (!entry.Value.HasAttachment) continue;
                    weight += entry.Value.Attachment.Weight;
                }

                return weight;
            }
        }

        public int Price => Inventory.Price.GetValue(this);
        public Currency Currency => this.Item != null ? this.Item.Price.Currency : null;
        public bool CanBuyFromMerchant => this.Item != null && this.Item.Price.CanBuyFromMerchant;
        public bool CanSellToMerchant => this.Item != null && this.Item.Price.CanSellToMerchant;

        public GameObject PropInstance { get; internal set; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public RuntimeItem()
        {
            this.m_Properties = new RuntimeProperties(this);
            this.m_Sockets = new RuntimeSockets(this);
        }
        
        public RuntimeItem(Item item)
        {
            this.m_ItemID = item != null ? item.ID : IdString.EMPTY;
            this.m_RuntimeID = new IdString($"{this.m_ItemID.String}-{Guid.NewGuid():N}");

            this.m_Properties = new RuntimeProperties(this);
            this.m_Sockets = new RuntimeSockets(this);
        }

        public RuntimeItem(RuntimeItem runtimeItem, bool withEmptySockets)
        {
            this.m_ItemID = runtimeItem.ItemID;
            this.m_RuntimeID = new IdString($"{this.m_ItemID.String}-{Guid.NewGuid():N}");

            this.m_Properties = new RuntimeProperties(this, runtimeItem);
            this.m_Sockets = new RuntimeSockets(this);
            
            if (!withEmptySockets) this.m_Sockets.CopyFrom(runtimeItem);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool InheritsFrom(IdString itemID)
        {
            if (string.IsNullOrEmpty(itemID.String)) return true;
            
            Item item = this.Item;
            while (item != null)
            {
                if (item.ID.Hash == itemID.Hash) return true;
                item = item.Parent;
            }

            return false;
        }

        public bool CanStack(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return false;
            if (this.ItemID.Hash != runtimeItem.ItemID.Hash) return false;

            foreach (KeyValuePair<IdString, RuntimeProperty> entry in this.m_Properties)
            {
                bool hasProperty = runtimeItem.m_Properties.TryGetValue(
                    entry.Key, 
                    out RuntimeProperty otherProperty
                );

                if (!hasProperty) return false;
                if (!entry.Value.Equivalent(otherProperty)) return false;
            }
            
            return this.m_Sockets.Count == 0 && runtimeItem.m_Sockets.Count == 0;
        }

        // EQUIPMENT METHODS: ---------------------------------------------------------------------
        
        internal async Task<bool> Equip()
        {
            if (this.Bag == null) return false;
            Args args = new Args(this.Bag.gameObject, this.Bag.Wearer);

            Bag_LastItemEquipped = this;
            bool conditions = Inventory.Equip.RunCanEquip(this.Item, args);

            if (!conditions) return false;
            await Inventory.Equip.RunOnEquip(this.Item, args);

            return true;
        }
        
        internal async Task<bool> Unequip()
        {
            if (this.Bag == null) return false;
            Args args = new Args(this.Bag.gameObject, this.Bag.Wearer);

            Bag_LastItemUnequipped = this;
            await Inventory.Equip.RunOnUnequip(this.Item, args);

            return true;
        }
        
        // USAGE METHODS. -------------------------------------------------------------------------

        internal bool CanUse()
        {
            if (this.Bag == null) return false;
            
            Character character = this.Bag.Wearer.Get<Character>();
            if (character != null && character.Busy.AreArmsBusy) return false;

            Cooldown cooldown = this.Bag.Cooldowns.GetCooldown(this.Item);
            if (cooldown is { IsReady: false }) return false;

            Args args = new Args(this.Bag.gameObject, this.Bag.Wearer);
            return Usage.RunCanUse(this.Item, args);
        }
        
        internal async Task<bool> Use()
        {
            Bag_LastItemAttemptedUse = this;
            if (!this.CanUse()) return false;
            
            Args args = new Args(this.Bag.gameObject, this.Bag.Wearer);

            Bag_LastItemUsed = this;

            Character character = this.Bag.Wearer != null 
                ? this.Bag.Wearer.Get<Character>()
                : null;
            
            if (character != null) character.Busy.MakeArmsBusy();
            
            this.Bag.Cooldowns.SetCooldown(this.Item, args);
            await Usage.RunOnUse(this.Item, args);
            
            if (character != null) character.Busy.RemoveArmsBusy();

            return true;
        }
        
        // DISMANTLING: ---------------------------------------------------------------------------

        internal void Dismantle(float chance, Bag outputBag = null)
        {
            outputBag ??= this.Bag;
            Crafting.Dismantle(this, this.Bag, outputBag, chance);
        }
    }
}