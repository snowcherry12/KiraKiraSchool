using System;

namespace GameCreator.Runtime.Inventory
{
    public interface IBagShape
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        int MaxWidth { get; }
        int MaxHeight { get; }
        int MaxWeight { get; }
        
        bool HasInfiniteWidth { get; }
        bool HasInfiniteHeight { get; }
        
        bool CanIncreaseWidth { get; }
        bool CanDecreaseWidth { get; }
        
        bool CanIncreaseHeight { get; }
        bool CanDecreaseHeight { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        event Action EventChange; 

        // INITIALIZERS: --------------------------------------------------------------------------

        void OnAwake(Bag bag);
        void OnLoad(TokenBagShape tokenBagShape);
        
        // METHODS: -------------------------------------------------------------------------------
        
        bool IncreaseWidth(int numColumns);
        bool DecreaseWidth(int numColumns);
        
        bool IncreaseHeight(int numRows);
        bool DecreaseHeight(int numRows);
    }
}