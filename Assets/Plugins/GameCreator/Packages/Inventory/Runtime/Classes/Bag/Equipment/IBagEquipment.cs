using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    public interface IBagEquipment
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public int Count { get; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<RuntimeItem, int> EventChange;
        public event Action<RuntimeItem, int> EventEquip;
        public event Action<RuntimeItem, int> EventUnequip;

        // INITIALIZERS: --------------------------------------------------------------------------

        void OnAwake(Bag bag);
        void OnLoad(TokenBagEquipment tokenBagEquipment);

        // CHECKERS: ------------------------------------------------------------------------------

        string GetSlotBaseName(int equipmentSlotIndex);
        IdString GetSlotBaseID(int equipmentSlotIndex);
        IdString GetSlotRootRuntimeItemID(int equipmentSlotIndex);

        int GetEquippedIndex(RuntimeItem runtimeItem);
        bool IsEquipped(RuntimeItem runtimeItem);
        bool IsEquippedType(Item item);

        bool CanEquip(RuntimeItem runtimeItem);
        bool CanEquip(RuntimeItem runtimeItem, int slot);
        bool CanEquipToIndex(RuntimeItem runtimeItem, int index);
        bool CanEquipType(Item item);

        // EQUIP: ---------------------------------------------------------------------------------
        
        Task<bool> Equip(RuntimeItem runtimeItem);
        Task<bool> Equip(RuntimeItem runtimeItem, int slot);
        Task<bool> EquipToIndex(RuntimeItem runtimeItem, int index);

        // UNEQUIP: -------------------------------------------------------------------------------
        
        Task<bool> Unequip(RuntimeItem runtimeItem);
        Task<bool> UnequipFromIndex(int index);
        Task<bool> UnequipType(IdString itemID, int slot);
        
        // SOCKETS: -------------------------------------------------------------------------------

        bool AttachTo(RuntimeItem runtimeItem, RuntimeItem attachment);
        bool AttachTo(RuntimeItem runtimeItem, RuntimeItem attachment, IdString socketID);
        
        RuntimeItem DetachFrom(RuntimeItem runtimeItem, RuntimeItem detachment);
        RuntimeItem DetachFrom(RuntimeItem runtimeItem, IdString socketID);
    }
}