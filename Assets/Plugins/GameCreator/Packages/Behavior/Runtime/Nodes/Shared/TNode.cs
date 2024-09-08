using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TNode : ISerializationCallbackReceiver
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private IdString m_Id; 
        [SerializeField] private Vector2 m_Position;
        
        [SerializeField] private Ports m_Ports;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector2 Position
        {
            get => this.m_Position;
            internal set => this.m_Position = value;
        }

        public IdString Id => this.m_Id;

        public abstract PropertyName TypeId { get; }

        public Ports Ports => this.m_Ports;

        public virtual Graph Subgraph => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TNode()
        {
            this.m_Id = new IdString(UniqueID.GenerateID());
            this.m_Ports = this.CreatePorts();
        }

        private Ports CreatePorts()
        {
            TInputPort[] inputs = this.CreateInputs();
            TOutputPort[] outputs = this.CreateOutputs();

            return new Ports(inputs, outputs);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public Status Run(Processor processor, Graph graph)
        {
            Status status = this.Update(processor, graph);
            processor.RuntimeData.SetStatus(this.Id, status);
            
            return status;
        }

        public void Abort(Processor processor, Graph graph)
        {
            this.Cancel(processor, graph);
        }

        public void ClearNodes(Processor processor, Graph graph)
        {
            processor.RuntimeData.SetStatus(this.Id, Status.Ready);

            if (this.Subgraph != null)
            {
                foreach (TNode subgraphNode in this.Subgraph.Nodes)
                {
                    subgraphNode?.ClearNodes(processor, graph);
                }
            }
            
            foreach (TOutputPort outputPort in this.Ports.Outputs)
            {
                foreach (Connection connection in outputPort.Connections)
                {
                    TNode subNode = graph.GetFromPortId(connection);
                    subNode?.ClearNodes(processor, graph);
                }
            }
        }
        
        // PUBLIC UTILITIES: ----------------------------------------------------------------------
        
        public Status GetStatus(Processor processor)
        {
            return processor.RuntimeData.GetStatus(this.Id);
        }

        public T GetValue<T>(Processor processor) where T : class, IValue
        {
            return processor.RuntimeData.GetValue<T>(this.Id);
        }

        public virtual void SetPortDirection(Vector2 direction)
        { }
        
        public virtual void CopyFrom(object source)
        { }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected abstract Status Update(Processor processor, Graph graph);
        protected abstract void Cancel(Processor processor, Graph graph);
        
        protected virtual TInputPort[] CreateInputs() => Array.Empty<TInputPort>();
        protected virtual TOutputPort[] CreateOutputs() => Array.Empty<TOutputPort>();

        // STRING METHODS: ------------------------------------------------------------------------

        public override string ToString() => $"{this.TypeId}:{this.Id}";

        // SERIALIZATION: -------------------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (this.m_Id.Hash == IdString.EMPTY.Hash)
            {
                string newId = UniqueID.GenerateID();
                this.m_Id = new IdString(newId);
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