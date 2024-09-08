using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeBehaviorTreeTask : TNodeBehaviorTree
    {
        public const string TYPE_ID = "behavior-tree:task";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Stop m_Stop = Stop.Immediately;
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_Instructions = new RunInstructionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // INPUT METHODS: -------------------------------------------------------------------------

        protected override TInputPort[] CreateInputs() => new TInputPort[]
        {
            new InputPortBehaviorTreeDefault()
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeBehaviorTreeTask sourceNode) return;
            
            this.m_Stop = sourceNode.m_Stop;
            this.m_Conditions = sourceNode.m_Conditions;
            this.m_Instructions = sourceNode.m_Instructions;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override Status Update(Processor processor, Graph graph)
        {
            Status status = this.GetStatus(processor);
            
            if (status == Status.Success) return Status.Success;
            if (status == Status.Failure) return Status.Failure;

            if (this.m_Conditions.Check(processor.Args))
            {
                ValueBehaviorTreeTask nodeData = this.GetValue<ValueBehaviorTreeTask>(processor);
                
                return status switch
                {
                    Status.Ready => this.RunInstructions(processor),
                    Status.Running => nodeData.IsRunning ? Status.Running : Status.Success,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            
            if (this.m_Stop == Stop.WhenComplete && this.GetStatus(processor) == Status.Running)
            {
                ValueBehaviorTreeTask nodeData = this.GetValue<ValueBehaviorTreeTask>(processor);
                if (nodeData?.IsRunning ?? false) return Status.Running;
            }

            this.Abort(processor, graph);
            return Status.Failure;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);
            this.GetValue<ValueBehaviorTreeTask>(processor)?.Abort();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Status RunInstructions(Processor processor)
        {
            ValueBehaviorTreeTask newData = new ValueBehaviorTreeTask();
            
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = this.Id.ToString(),
                Cancellable = newData
            };
                        
            newData.Task = this.m_Instructions.Run(processor.Args, runnerConfig);
            processor.RuntimeData.SetValue(this.Id, newData);
            
            return Status.Running;
        }
    }
}