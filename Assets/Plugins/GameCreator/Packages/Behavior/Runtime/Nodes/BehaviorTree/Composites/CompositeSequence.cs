using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Sequence")]
    [Category("Sequence")]
    
    [Image(typeof(IconCompositeSequence), ColorTheme.Type.TextLight)]
    [Description("Executes its child nodes in sequence as long as they are successful")]
    
    [Serializable]
    public class CompositeSequence : TComposite
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
                
                if (abortNextNodes)
                {
                    subNode.Abort(processor, graph);
                    continue;
                }
                
                status = subNode.Run(processor, graph);

                if (status == Status.Failure)
                {
                    abortNextNodes = true;
                    continue;
                }

                if (status == Status.Running)
                {
                    break;
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