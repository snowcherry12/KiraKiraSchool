using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Selector")]
    [Category("Selector")]
    
    [Image(typeof(IconCompositeSelector), ColorTheme.Type.TextLight)]
    [Description("Executes the first successful node from its children")]
    
    [Serializable]
    public class CompositeSelector : TComposite
    {
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override Status Run(TNode node, Processor processor, Graph graph)
        {
            if (processor.RuntimeData.GetStatus(node.Id) == Status.Ready)
            {
                this.Initialize(node, processor, graph);
            }
            
            ValueBehaviorTreeShuffle value = node.GetValue<ValueBehaviorTreeShuffle>(processor);
            
            Status status = Status.Success;
            bool abortNextNodes = false;

            if (node.Ports.Outputs.Length == 0) return status;
            TOutputPort output = node.Ports.Outputs[0]; 

            for (int i = 0; i < output.Connections.Length; ++i)
            {
                int index = this.GetIndex(i, value);
                TNode subNode = graph.GetFromPortId(output.Connections[index]);

                Status subNodeStatus = subNode.GetStatus(processor);
                if (subNodeStatus == Status.Failure) continue;
                
                if (abortNextNodes)
                {
                    subNode.Abort(processor, graph);
                    continue;
                }

                status = subNode.Run(processor, graph);

                if (status == Status.Running || status == Status.Success)
                {
                    abortNextNodes = true;
                }
            }

            return status;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual int GetIndex(int index, ValueBehaviorTreeShuffle value)
        {
            return index;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Initialize(TNode node, Processor processor, Graph graph)
        {
            ValueBehaviorTreeShuffle value = node.GetValue<ValueBehaviorTreeShuffle>(processor) ?? new ValueBehaviorTreeShuffle();
            
            int outputsLength = node.Ports.Outputs[0].Connections.Length;
            value.Shuffle(outputsLength);
            
            processor.RuntimeData.SetValue(node.Id, value);
        }
    }
}