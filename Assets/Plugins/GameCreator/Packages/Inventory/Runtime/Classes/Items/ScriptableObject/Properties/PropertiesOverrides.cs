using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class PropertiesOverrides : TSerializableDictionary<IdString, PropertyOverride>
    { }
}