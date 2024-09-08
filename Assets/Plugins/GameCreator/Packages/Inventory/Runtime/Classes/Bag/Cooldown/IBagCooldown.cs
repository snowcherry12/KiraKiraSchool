using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    public interface IBagCooldowns
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        Cooldowns Cooldowns { get; }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        void OnAwake(Bag bag);
        void OnLoad(TokenBagCooldowns tokenBagCooldowns);

        // GETTER METHODS: ------------------------------------------------------------------------
        
        Cooldown GetCooldown(Item item);

        // SETTER METHODS: ------------------------------------------------------------------------
        
        void SetCooldown(Item item, Args args);
        void ResetCooldown(Item item);
        void ClearCooldowns();
    }
}