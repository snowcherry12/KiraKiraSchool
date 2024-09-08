using System;

namespace GameCreator.Runtime.Stats
{
    internal class Operator
    {
        [field:NonSerialized] public Function Type { get; }
        [field:NonSerialized] public int Precedence { get; }
        [field:NonSerialized] public Associativity Associativity { get; }
        [field:NonSerialized] public int Inputs { get; }

        public Operator(Function type, int precedence, int inputs, Associativity associativity)
        {
            this.Type = type;
            this.Precedence = precedence;
            this.Inputs = inputs;
            this.Associativity = associativity;
        }
    }
}