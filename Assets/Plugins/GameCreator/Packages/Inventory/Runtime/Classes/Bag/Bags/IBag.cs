namespace GameCreator.Runtime.Inventory
{
    public interface IBag
    {
        IBagShape Shape { get; }
        IBagContent Content { get; }
        IBagEquipment Equipment { get; }
        IBagCooldowns Cooldowns { get; }

        void OnAwake(Bag bag);
    }
}