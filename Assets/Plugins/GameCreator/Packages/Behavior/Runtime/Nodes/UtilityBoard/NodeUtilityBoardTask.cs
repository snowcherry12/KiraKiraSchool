using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeUtilityBoardTask : TNodeUtilityBoard
    {
        public const string TYPE_ID = "utility-board:task";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private string m_Name = "My Task";
        
        [SerializeField] private RunInstructionsList m_OnExit = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_Instructions = new RunInstructionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override PropertyName TypeId => TYPE_ID;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not NodeUtilityBoardTask sourceNode) return;
            
            this.m_Name = sourceNode.m_Name;
            this.m_OnExit = sourceNode.m_OnExit;
            this.m_Instructions = sourceNode.m_Instructions;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override IValueWithScore RequireData(Processor processor)
        {
            IValueWithScore nodeData = this.GetValue<IValueWithScore>(processor);
            if (nodeData != null) return nodeData;

            nodeData = new ValueUtilityBoardTask();
            processor.RuntimeData.SetValue(this.Id, nodeData);
            
            return nodeData;
        }

        protected override Status Update(Processor processor, Graph graph)
        {
            Status status = this.GetStatus(processor);
            
            if (status == Status.Success) return Status.Success;
            if (status == Status.Failure) return Status.Failure;

            if (status == Status.Ready)
            {
                this.RunOnExecute(processor);
            }
            
            ValueUtilityBoardTask nodeData = this.GetValue<ValueUtilityBoardTask>(processor);
            if (nodeData?.IsRunning ?? false) return Status.Running;
            
            this.RunOnExit(processor);
            return Status.Success;
        }

        protected override void Cancel(Processor processor, Graph graph)
        {
            base.Cancel(processor, graph);

            ValueUtilityBoardTask data = this.GetValue<ValueUtilityBoardTask>(processor);
            if (data == null) return;
            
            if (data.IsRunning) this.RunOnExit(processor);
            this.GetValue<ValueUtilityBoardTask>(processor).Abort();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RunOnExit(Processor processor)
        {
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = $"Exit:{this.Id}"
            };
            
            _ = this.m_OnExit.Run(processor.Args, runnerConfig);
        }
        
        private void RunOnExecute(Processor processor)
        {
            ValueUtilityBoardTask newData = new ValueUtilityBoardTask();
            
            RunnerConfig runnerConfig = new RunnerConfig
            {
                Name = $"Execute:{this.Id}",
                Cancellable = newData
            };
            
            newData.Task = this.m_Instructions.Run(processor.Args, runnerConfig);
            processor.RuntimeData.SetValue(this.Id, newData);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => $"{this.m_Name}:{this.Id}";
    }
}