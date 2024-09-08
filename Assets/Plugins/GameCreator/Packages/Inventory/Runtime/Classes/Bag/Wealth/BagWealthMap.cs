using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagWealthMap : TSerializableDictionary<IdString, int>
    { }
}