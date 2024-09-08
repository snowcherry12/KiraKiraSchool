using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagEquipment : IBagEquipment, ISerializationCallbackReceiver
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Equipment m_Equipment;
        [SerializeField] private EquipmentRuntime m_EquipmentRuntime = new EquipmentRuntime();

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] protected Bag Bag { get; private set; }

        [field: NonSerialized] protected Dictionary<IdString, List<int>> Equipment { get; private set; }

        public int Count => this.m_Equipment != null && this.m_EquipmentRuntime != null
            ? this.m_Equipment.Slots.Length
            : 0;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<RuntimeItem, int> EventChange;
        public event Action<RuntimeItem, int> EventEquip;
        public event Action<RuntimeItem, int> EventUnequip;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void OnAwake(Bag bag)
        {
            this.Bag = bag;
            this.InitializeEquipment();
        }

        public void OnLoad(TokenBagEquipment tokenBagEquipment)
        {
            int equipmentLength = this.Count;
            for (int i = 0; i < equipmentLength; ++i)
            {
                _ = this.UnequipFromIndex(i);
            }

            for (int i = 0; i < tokenBagEquipment.Equipment.Length; ++i)
            {
                IdString runtimeItemID = tokenBagEquipment.Equipment[i]; 
                if (string.IsNullOrEmpty(runtimeItemID.String)) continue;

                RuntimeItem runtimeItem = this.Bag.Content.GetRuntimeItem(runtimeItemID);
                if (runtimeItem == null) continue;

                _ = this.EquipToIndex(runtimeItem, i);
            }
        }

        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void InitializeEquipment()
        {
            this.Equipment = new Dictionary<IdString, List<int>>();
            if (this.m_Equipment == null || this.m_Equipment.Slots == null) return;
            
            for (int i = 0; i < this.Count; ++i)
            {
                EquipmentSlot equipmentSlot = this.m_Equipment.Slots[i];
                IdString baseID = equipmentSlot.Base != null 
                    ? equipmentSlot.Base.ID
                    : IdString.EMPTY;

                if (!this.Equipment.ContainsKey(baseID))
                {
                    this.Equipment.Add(baseID, new List<int>());
                }

                this.Equipment[baseID].Add(i);
            }
        }

        // CHECKERS: ------------------------------------------------------------------------------

        public string GetSlotBaseName(int equipmentSlotIndex)
        {
            if (this.m_Equipment == null) return string.Empty;
            if (this.Count <= equipmentSlotIndex) return string.Empty;
            
            Item item = this.m_Equipment.Slots[equipmentSlotIndex]?.Base; 
            return item != null ? item.name : string.Empty;
        }
        
        public IdString GetSlotBaseID(int equipmentSlotIndex)
        {
            if (this.m_Equipment == null) return IdString.EMPTY;
            if (this.Count <= equipmentSlotIndex) return IdString.EMPTY;
            
            Item item = this.m_Equipment.Slots[equipmentSlotIndex]?.Base; 
            return item != null ? item.ID : IdString.EMPTY;
        }

        public IdString GetSlotRootRuntimeItemID(int equipmentSlotIndex)
        {
            if (this.Count <= equipmentSlotIndex) return IdString.EMPTY;

            bool hasIndex = this.m_EquipmentRuntime.TryGetValue(
                equipmentSlotIndex,
                out EquipmentRuntimeSlot slot
            );
            
            return hasIndex ? slot.RootRuntimeItemIDEquipped : IdString.EMPTY;
        }
        
        public int GetEquippedIndex(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return -1;
            if (!this.Bag.Content.Contains(runtimeItem)) return -1;
            if (!runtimeItem.Item.Equip.IsEquippable) return -1;

            foreach (KeyValuePair<int, EquipmentRuntimeSlot> entry in this.m_EquipmentRuntime)
            {
                if (!entry.Value.IsEquipped) continue;
                if (entry.Value.RootRuntimeItemIDEquipped.Hash == runtimeItem.RuntimeID.Hash)
                {
                    return entry.Key;
                }
            }

            return -1;
        }
        
        public bool IsEquipped(RuntimeItem runtimeItem)
        {
            return this.GetEquippedIndex(runtimeItem) >= 0;
        }

        public bool IsEquippedType(Item item)
        {
            if (item == null) return false;
            if (!this.Bag.Content.ContainsType(item, 1)) return false;

            foreach (KeyValuePair<int, EquipmentRuntimeSlot> entry in this.m_EquipmentRuntime)
            {
                if (!entry.Value.IsEquipped) continue;
                RuntimeItem runtimeItem = this.Bag
                    .Content
                    .GetRuntimeItem(entry.Value.RootRuntimeItemIDEquipped);
                
                if (runtimeItem != null && runtimeItem.InheritsFrom(item.ID)) return true;
            }

            return false;
        }

        public bool CanEquip(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return false;
            if (this.IsEquipped(runtimeItem)) return false;
            if (!this.Bag.Content.Contains(runtimeItem)) return false;
            if (!runtimeItem.Item.Equip.IsEquippable) return false;

            if (this.m_Equipment == null) return false;
            foreach (EquipmentSlot slot in this.m_Equipment.Slots)
            {
                if (slot.Base == null) return false;
                if (runtimeItem.InheritsFrom(slot.Base.ID)) return true;
            }

            return false;
        }
        
        public bool CanEquip(RuntimeItem runtimeItem, int slot)
        {
            if (runtimeItem == null) return false;
            if (!this.Bag.Content.Contains(runtimeItem)) return false;

            IdString itemID = this.FindBase(runtimeItem.ItemID);
            if (string.IsNullOrEmpty(itemID.String)) return false;
            
            if (slot < 0) slot = this.FindFreeSlot(itemID);
            if (slot < 0) return false;
            
            int index = this.Equipment[itemID][slot];
            return this.CanEquipToIndex(runtimeItem, index);
        }
        
        public bool CanEquipToIndex(RuntimeItem runtimeItem, int index)
        {
            if (runtimeItem == null) return false;
            if (this.IsEquipped(runtimeItem)) return false;
            if (!this.Bag.Content.Contains(runtimeItem)) return false;
            if (!runtimeItem.Item.Equip.IsEquippable) return false;

            if (this.m_Equipment == null || this.m_Equipment.Slots == null) return false;
            if (this.m_Equipment.Slots.Length <= index) return false;
            
            EquipmentSlot slot = this.m_Equipment.Slots[index];
            return slot.Base != null && runtimeItem.InheritsFrom(slot.Base.ID);
        }

        public bool CanEquipType(Item item)
        {
            if (item == null) return false;
            if (!this.Bag.Content.ContainsType(item, 1)) return false;
            if (!item.Equip.IsEquippable) return false;

            if (this.m_Equipment == null || this.m_Equipment.Slots == null) return false;
            foreach (EquipmentSlot slot in this.m_Equipment.Slots)
            {
                if (slot.Base == null) return false;
                if (item.InheritsFrom(slot.Base.ID)) return true;
            }

            return false;
        }

        // EQUIP: ---------------------------------------------------------------------------------
        
        public async Task<bool> Equip(RuntimeItem runtimeItem)
        {
            return await this.Equip(runtimeItem, -1);
        }

        public async Task<bool> Equip(RuntimeItem runtimeItem, int slot)
        {
            if (!this.CanEquip(runtimeItem, slot)) return false;
            
            IdString itemID = this.FindBase(runtimeItem.ItemID);
            if (string.IsNullOrEmpty(itemID.String)) return false;
            
            if (slot < 0) slot = this.FindFreeSlot(itemID);
            if (slot < 0) return false;
            
            int index = this.Equipment[itemID][slot];
            return await this.EquipToIndex(runtimeItem, index);
        }

        public async Task<bool> EquipToIndex(RuntimeItem runtimeItem, int index)
        {
            if (!this.CanEquipToIndex(runtimeItem, index)) return false;

            if (index >= this.Count) return false;
            if (index < 0) return false;

            if (this.m_Equipment == null || this.m_Equipment.Slots == null) return false;
            EquipmentSlot slot = this.m_Equipment.Slots[index];
            
            bool hasIndex = this.m_EquipmentRuntime.TryGetValue(
                index,
                out EquipmentRuntimeSlot runtimeSlot
            );
            
            if (!hasIndex) return false;

            if (runtimeSlot.IsEquipped)
            {
                IdString runtimeItemIDEquipped = runtimeSlot.RootRuntimeItemIDEquipped;
                RuntimeItem runtimeItemEquipped = this.Bag
                    .Content
                    .GetRuntimeItem(runtimeItemIDEquipped);
                
                await this.Unequip(runtimeItemEquipped);
            }

            if (this.IsEquipped(runtimeItem)) await runtimeItem.Unequip();
            if (await runtimeItem.Equip())
            {
                Args args = new Args(this.Bag.gameObject, this.Bag.Wearer);
                runtimeSlot.Equip(this.Bag, slot.Get(args), runtimeItem);
            }
            
            this.EventEquip?.Invoke(runtimeItem, index);
            this.EventChange?.Invoke(runtimeItem, index);
            return true;
        }

        // UNEQUIP: -------------------------------------------------------------------------------

        public async Task<bool> Unequip(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return false;
            foreach (KeyValuePair<int, EquipmentRuntimeSlot> entry in this.m_EquipmentRuntime)
            {
                if (entry.Value.RootRuntimeItemIDEquipped.Hash != runtimeItem.RuntimeID.Hash) continue;

                bool unequipSuccess = await runtimeItem.Unequip();
                if (!unequipSuccess) continue;
                
                entry.Value.Unequip(this.Bag);

                this.EventUnequip?.Invoke(runtimeItem, entry.Key);
                this.EventChange?.Invoke(runtimeItem, entry.Key);
                return true;
            }
            
            return false;
        }
        
        public async Task<bool> UnequipFromIndex(int index)
        {
            if (index < 0 || index >= this.Count) return false;

            bool hasIndex = this.m_EquipmentRuntime.TryGetValue(
                index,
                out EquipmentRuntimeSlot runtimeSlot
            );
            
            if (!hasIndex) return false;
            if (!runtimeSlot.IsEquipped) return false;

            IdString runtimeItemID = runtimeSlot.RootRuntimeItemIDEquipped; 
            RuntimeItem runtimeItem = this.Bag.Content.GetRuntimeItem(runtimeItemID);
            
            if (runtimeItem == null) return false;
            return await this.Unequip(runtimeItem);
        }

        public async Task<bool> UnequipType(IdString itemID, int slot)
        {
            itemID = this.FindBase(itemID);
            if (string.IsNullOrEmpty(itemID.String)) return false;

            int index = this.Equipment[itemID][slot];
            bool hasIndex = this.m_EquipmentRuntime.TryGetValue(
                index,
                out EquipmentRuntimeSlot runtimeSlot
            );
            
            if (!hasIndex) return false;
            if (!runtimeSlot.IsEquipped) return false;
            
            IdString runtimeItemIDEquipped = runtimeSlot.RootRuntimeItemIDEquipped;
            RuntimeItem runtimeItemEquipped = this.Bag
                .Content
                .GetRuntimeItem(runtimeItemIDEquipped);

            if (runtimeItemEquipped == null) return false;
            
            bool unequipSuccess = await runtimeItemEquipped.Unequip();
            if (!unequipSuccess) return false;
            
            runtimeSlot.Unequip(this.Bag);
                
            this.EventUnequip?.Invoke(runtimeItemEquipped, index);
            this.EventChange?.Invoke(runtimeItemEquipped, index);
                
            return true;

        }

        // SOCKETS: -------------------------------------------------------------------------------

        public bool AttachTo(RuntimeItem runtimeItem, RuntimeItem attachment)
        {
            if (runtimeItem == null) return false;
            
            IdString socketID = IdString.EMPTY;
            foreach (KeyValuePair<IdString, RuntimeSocket> entry in runtimeItem.Sockets)
            {
                if (entry.Value.CanAttach(attachment))
                {
                    socketID = entry.Key;
                    if (!entry.Value.HasAttachment) break;
                }
            }

            if (string.IsNullOrEmpty(socketID.String)) return false;
            return this.AttachTo(runtimeItem, attachment, socketID);
        }

        public bool AttachTo(RuntimeItem runtimeItem, RuntimeItem attachment, IdString socketID)
        {
            if (attachment == null) return false;
            if (runtimeItem == null) return false;
            if (!runtimeItem.Sockets.ContainsKey(socketID)) return false;

            IBagEquipment equipment = this.Bag.Equipment;
            int equipIndex = -1;
            
            if (equipment.IsEquipped(attachment)) equipment.Unequip(attachment);
            if (equipment.IsEquipped(runtimeItem))
            {
                equipIndex = equipment.GetEquippedIndex(runtimeItem);
                equipment.Unequip(runtimeItem);
            }

            bool success = runtimeItem.Sockets.AttachToSocket(runtimeItem, socketID, attachment);

            if (success)
            {
                this.Bag.Content.Remove(attachment);
                this.EventChange?.Invoke(runtimeItem, equipIndex);
            }

            if (equipIndex >= 0) equipment.EquipToIndex(runtimeItem, equipIndex);
            return success;
        }

        public RuntimeItem DetachFrom(RuntimeItem runtimeItem, RuntimeItem detachment)
        {
            if (runtimeItem == null) return null;
            if (detachment == null) return null;
            
            foreach (KeyValuePair<IdString, RuntimeSocket> entry in runtimeItem.Sockets)
            {
                if (!entry.Value.HasAttachment) continue;
                if (entry.Value.Attachment.RuntimeID.Hash != detachment.RuntimeID.Hash) continue;
                
                IdString socketID = entry.Key;
                return this.DetachFrom(runtimeItem, socketID);
            }

            return null;
        }

        public RuntimeItem DetachFrom(RuntimeItem runtimeItem, IdString socketID)
        {
            if (runtimeItem == null) return null;
            if (!runtimeItem.Sockets.ContainsKey(socketID)) return null;

            IBagEquipment equipment = this.Bag.Equipment;
            int equipIndex = -1;
            
            if (equipment.IsEquipped(runtimeItem))
            {
                equipIndex = equipment.GetEquippedIndex(runtimeItem);
                equipment.Unequip(runtimeItem);
            }
            
            RuntimeItem attachment = runtimeItem.Sockets.DetachFromSocket(runtimeItem, socketID);
            if (attachment != null) this.EventChange?.Invoke(runtimeItem, equipIndex);
            
            if (equipIndex >= 0) equipment.EquipToIndex(runtimeItem, equipIndex);
            return attachment;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private int FindFreeSlot(IdString itemID)
        {
            if (!this.Equipment.TryGetValue(itemID, out List<int> slots)) return -1;
            
            for (int i = 0; i < slots.Count; ++i)
            {
                int index = slots[i];
                
                bool hasIndex = this.m_EquipmentRuntime.TryGetValue(
                    index,
                    out EquipmentRuntimeSlot runtimeSlot
                );

                if (hasIndex && !runtimeSlot.IsEquipped) return i;
            }

            return 0;
        }

        private IdString FindBase(IdString itemID)
        {
            Item item = Settings.From<InventoryRepository>().Items.Get(itemID);
            if (item == null) return IdString.EMPTY;

            if (this.m_Equipment == null || this.m_Equipment.Slots == null) return IdString.EMPTY;
            foreach (EquipmentSlot slot in this.m_Equipment.Slots)
            {
                if (slot.Base == null) continue;
                if (item.InheritsFrom(slot.Base.ID)) return slot.Base.ID;
            }
            
            return IdString.EMPTY;
        }

        private EquipmentRuntimeSlot GetEquipmentRuntimeSlot(int index)
        {
            if (this.m_Equipment == null) return null;

            if (index >= this.Count) return null;
            if (index < 0) return null;

            bool hasIndex = this.m_EquipmentRuntime.TryGetValue(index, out EquipmentRuntimeSlot runtimeSlot);
            return hasIndex ? runtimeSlot : null;
        }

        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            this.m_EquipmentRuntime.SyncWithEquipment(this.m_Equipment);
        }
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}