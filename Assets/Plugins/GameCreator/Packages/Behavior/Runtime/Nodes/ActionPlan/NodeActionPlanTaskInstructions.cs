using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeActionPlanTaskInstructions : TNodeActionPlanTask
    {
        public const string TYPE_ID = "action-plan:task";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private string m_Name = "My State";
        
        [SerializeField] private RunInstructionsList m_Instructions = new RunInstructionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override PropertyName TypeId => TYPE_ID;

        protected override string Name => string.IsNullOrEmpty(this.m_Name) ? "Task" : this.m_Name;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeActionPlanTaskInstructions sourceNode) return;
            
            this.m_Instructions = sourceNode.m_Instructions;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            Status status = this.GetStatus(processor);
            
            if (status == Status.Success) return Status.Success;
            if (status == Status.Failure) return Status.Failure;

            if (status == Status.Ready)
            {
                return this.CheckConditions(processor.Args) 
                    ? this.RunInstructions(processor) 
                    : Status.Failure;
            }

            ValueActionPlanTaskInstructions nodeData = this.GetValue<ValueActionPlanTaskInstructions>(processor);
            return nodeData.IsRunning ? Status.Running : Status.Success;
        }
        
        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            this.GetValue<ValueActionPlanTaskInstructions>(processor)?.Abort();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private Status RunInstructions(Processor processor)
        {
            ValueActionPlanTaskInstructions newData = new ValueActionPlanTaskInstructions();
            
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = this.Id.ToString(),
                Cancellable = newData
            };
                        
            newData.Task = this.m_Instructions.Run(processor.Args, runnerConfig);
            processor.RuntimeData.SetValue(this.Id, newData);
            
            return Status.Running;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => $"{this.m_Name}:{this.Id}";
    }
}