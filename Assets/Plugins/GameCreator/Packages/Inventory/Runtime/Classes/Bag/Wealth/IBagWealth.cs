using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    public interface IBagWealth
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        List<IdString> List { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        event Action<IdString, int, int> EventChange;

        // INITIALIZERS: --------------------------------------------------------------------------

        void OnLoad(TokenBagWealth tokenBagWealth);
        
        // GETTERS: -------------------------------------------------------------------------------

        int Get(Currency currency);
        int Get(IdString currencyID);

        // SETTERS: -------------------------------------------------------------------------------

        void Set(Currency currency, int value);
        void Set(IdString currencyID, int value);

        void Add(Currency currency, int value);
        void Add(IdString currencyID, int value);

        void Subtract(Currency currency, int value);
        void Subtract(IdString currencyID, int value);
    }
}