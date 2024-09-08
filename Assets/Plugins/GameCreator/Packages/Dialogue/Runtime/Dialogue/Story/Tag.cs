using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Dialogue
{
    public struct Tag
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public IdString Name { get; }
        [field: NonSerialized] public int NodeId { get; }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Tag(IdString tag, int nodeId)
        {
            this.Name = tag;
            this.NodeId = nodeId;
        }
    }
}