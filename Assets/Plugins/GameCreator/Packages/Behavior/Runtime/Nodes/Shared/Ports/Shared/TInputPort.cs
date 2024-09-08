using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TInputPort : TPort
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public sealed override PortMode Mode => PortMode.Input;
        
        protected abstract Type[] AcceptTypes { get; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual bool CanConnectFrom(TOutputPort fromOutputPort)
        {
            if (fromOutputPort == null) return false;

            foreach (Connection connection in fromOutputPort.Connections)
            {
                if (connection == this.Id) return false;
            }
            
            foreach (Type acceptType in this.AcceptTypes)
            {
                if (fromOutputPort.GetType() == acceptType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}