using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeStateMachineState : TNodeStateMachine
    {
        public const string TYPE_ID = "state-machine:state";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private string m_Name = "My State";

        [SerializeField] private Check m_Check = Check.EveryCycle;
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();

        [SerializeField] private RunInstructionsList m_OnEnter = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnExit = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_Instructions = new RunInstructionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortStateMachineMultiple()
        };

        protected override TOutputPort[] CreateOutputs() => new TOutputPort[]
        {
            new OutputPortStateMachineMultiple()
        };

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeStateMachineState sourceNode) return;
            
            this.m_Name = sourceNode.m_Name;
            this.m_Check = sourceNode.m_Check;
            this.m_Conditions = sourceNode.m_Conditions;
            
            this.m_OnEnter = sourceNode.m_OnEnter;
            this.m_OnExit = sourceNode.m_OnExit;
            this.m_Instructions = sourceNode.m_Instructions;
        }

        public override bool TryRun(Processor processor, Graph graph, BeforeRunHandle beforeEnter)
        {
            if (!this.m_Conditions.Check(processor.Args)) return false;
            
            beforeEnter?.Invoke(processor);
            this.Run(processor, graph);
            
            return true;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            this.SetAsCurrentIndex(processor, graph);
            Status status = this.GetStatus(processor);

            ValueStateMachineState nodeData = this.GetValue<ValueStateMachineState>(processor);
            bool isRunning = nodeData?.IsRunning ?? false;

            if (status == Status.Running && !isRunning)
            {
                if (this.m_Check == Check.EveryCycle)
                {
                    TOutputPort outputs = this.Ports.Outputs[0];
                    foreach (Connection connection in outputs.Connections)
                    {
                        if (graph.GetFromPortId(connection) is not TNodeStateMachine candidate) continue;
                        if (candidate.TryRun(processor, graph, this.RunOnExit))
                        {
                            this.Abort(processor, graph);
                            return Status.Ready;
                        }
                    }
                }
            }
            
            if (status == Status.Ready)
            {
                this.RunOnEnter(processor);
            }

            if (isRunning)
            {
                if (this.m_Check == Check.EveryFrame)
                {
                    TOutputPort outputs = this.Ports.Outputs[0]; 
                    foreach (Connection connection in outputs.Connections)
                    {
                        if (graph.GetFromPortId(connection) is not TNodeStateMachine candidate) continue;
                        if (candidate.TryRun(processor, graph, this.RunOnExit))
                        {
                            this.Abort(processor, graph);
                            return Status.Ready;
                        }
                    }
                }
                
                return Status.Running;
            }

            this.RunOnUpdate(processor);
            return Status.Running;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            this.GetValue<ValueStateMachineState>(processor)?.Abort();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RunOnEnter(Processor processor)
        {
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = $"Enter:{this.Id}"
            };
            
            _ = this.m_OnEnter.Run(processor.Args, runnerConfig);
        }
        
        private void RunOnExit(Processor processor)
        {
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = $"Exit:{this.Id}"
            };
            
            _ = this.m_OnExit.Run(processor.Args, runnerConfig);
        }
        
        private void RunOnUpdate(Processor processor)
        {
            ValueStateMachineState newData = new ValueStateMachineState();
            
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = $"Update:{this.Id}",
                Cancellable = newData
            };
            
            newData.Task = this.m_Instructions.Run(processor.Args, runnerConfig);
            processor.RuntimeData.SetValue(this.Id, newData);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => $"{this.m_Name}:{this.Id}";
    }
}