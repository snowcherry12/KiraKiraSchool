using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    public class Step
    {
        public IdString Id { get; }
        public Step Parent { get; }
        
        public float Cost { get; }
        public State ResolveState { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public Step(IdString id, Step parent, float cost, State state)
        {
            this.Id = id;
            this.Parent = parent;
            
            this.Cost = cost;
            this.ResolveState = state.Copy();
        }
        
        // public Step(Step parent, float cost, State allStates, Beliefs beliefs, IdString id) {
        //
        //     this.Parent = parent;
        //     this.Cost = cost;
        //     this.ResolveState = allStates.Copy();
        //
        //     //as well as the world states add the agents beliefs as states that can be
        //     //used to match preconditions
        //     this.ResolveState.Apply(beliefs);
        //     
        //     // foreach (KeyValuePair<string, int> b in beliefStates) {
        //     //
        //     //     if (!this.state.ContainsKey(b.Key)) {
        //     //
        //     //         this.state.Add(b.Key, b.Value);
        //     //     }
        //     // }
        //     
        //     this.Id = id;
        // }        
    }
}