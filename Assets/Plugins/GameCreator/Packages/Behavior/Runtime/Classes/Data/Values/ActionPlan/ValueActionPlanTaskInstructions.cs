using System;
using System.Threading.Tasks;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Behavior
{
    public class ValueActionPlanTaskInstructions : IValue, ICancellable
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public bool IsRunning => this.Task != null && this.Task.Status switch
        {
            TaskStatus.Created => true,
            TaskStatus.WaitingForActivation => true,
            TaskStatus.WaitingToRun => true,
            TaskStatus.Running => true,
            TaskStatus.WaitingForChildrenToComplete => true,
            TaskStatus.RanToCompletion => false,
            TaskStatus.Canceled => false,
            TaskStatus.Faulted => false,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        [field: NonSerialized] public Task Task { private get; set; }
        [field: NonSerialized] public bool IsCancelled { get; private set; }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ValueActionPlanTaskInstructions()
        {
            this.Task = null;
            this.IsCancelled = false;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Abort()
        {
            this.IsCancelled = true;
            this.Task = null;
        }
        
        public void Restart()
        {
            this.IsCancelled = false;
            this.Task = null;
        }
    }
}