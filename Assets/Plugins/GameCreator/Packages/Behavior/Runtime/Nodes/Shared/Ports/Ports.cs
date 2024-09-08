using System;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class Ports
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private TInputPort[] m_Inputs = Array.Empty<TInputPort>();
        [SerializeReference] private TOutputPort[] m_Outputs = Array.Empty<TOutputPort>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public TInputPort[] Inputs => this.m_Inputs;
        public TOutputPort[] Outputs => this.m_Outputs;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Ports()
        { }

        public Ports(TInputPort[] inputs, TOutputPort[] outputs) : this()
        {
            this.m_Inputs = inputs;
            this.m_Outputs = outputs;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public T GetInput<T>(Connection portId) where T : TInputPort
        {
            foreach (TInputPort input in this.m_Inputs)
            {
                if (input.Id == portId) return input as T;
            }

            return null;
        }
        
        public T GetOutput<T>(Connection portId) where T : TOutputPort
        {
            foreach (TOutputPort output in this.m_Outputs)
            {
                if (output.Id == portId) return output as T;
            }

            return null;
        }
    }
}