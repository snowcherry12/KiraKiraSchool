using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Cooldowns : TSerializableDictionary<IdString, Cooldown>
    { }
}