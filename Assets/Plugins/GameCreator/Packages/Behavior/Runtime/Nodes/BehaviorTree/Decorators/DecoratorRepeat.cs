using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Repeat")]
    [Category("Repeat")]
    
    [Image(typeof(IconDecoratorRepeat), ColorTheme.Type.TextLight)]
    [Description("Allows to execute its child node up to a certain amount of times")]
    
    [Serializable]
    public class DecoratorRepeat : TDecorator
    {
        [SerializeField] private PropertyGetInteger m_Times = GetDecimalInteger.Create(5);
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool Conditions(TNode node, Processor processor, Graph graph)
        {
            int currentCycles = node.GetValue<ValueBehaviorTreeCycles>(processor)?.Cycles ?? 0;
            return currentCycles < this.MaxCycles(processor.Args);
        }
        
        public override Status Run(Status status, TNode node, Processor processor, Graph graph)
        {
            if (status != Status.Success && status != Status.Failure) return status;
            
            ValueBehaviorTreeCycles value = node.GetValue<ValueBehaviorTreeCycles>(processor) ?? new ValueBehaviorTreeCycles();
            
            value.Increase();
            processor.RuntimeData.SetValue(node.Id, value);
            
            if (value.Cycles >= this.MaxCycles(processor.Args))
            {
                return status;
            }

            node.ClearNodes(processor, graph);
            return Status.Running;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private int MaxCycles(Args args)
        {
            int maxCycles = (int) this.m_Times.Get(args);
            return Math.Max(1, maxCycles);
        }
    }
}