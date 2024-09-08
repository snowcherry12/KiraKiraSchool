using System;

namespace GameCreator.Runtime.Behavior
{
    public class ValueBehaviorTreeCycles : IValue
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public int Cycles { get; private set; }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ValueBehaviorTreeCycles()
        {
            this.Cycles = 0;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Increase()
        {
            this.Cycles += 1;
        }
        
        public void Restart()
        {
            this.Cycles = 0;
        }
    }
}