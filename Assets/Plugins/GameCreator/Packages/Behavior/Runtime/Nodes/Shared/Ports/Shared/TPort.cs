using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TPort : ISerializationCallbackReceiver
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Connection m_Id;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Connection Id => this.m_Id;

        public abstract PortMode Mode { get; }
        public abstract PortAllowance Allowance { get; }

        public abstract WireShape WireShape { get; }
        
        public abstract PortPosition Position { get; set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TPort()
        {
            string newId = UniqueID.GenerateID();
            this.m_Id = new Connection(newId);
        }

        // SERIALIZATION: -------------------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (this.m_Id.Value == string.Empty)
            {
                string newId = UniqueID.GenerateID();
                this.m_Id = new Connection(newId);
            }
            
            this.RunBeforeSerialize();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.RunAfterSerialize();
        }

        protected virtual void RunBeforeSerialize()
        { }
        
        protected virtual void RunAfterSerialize()
        { }
    }
}