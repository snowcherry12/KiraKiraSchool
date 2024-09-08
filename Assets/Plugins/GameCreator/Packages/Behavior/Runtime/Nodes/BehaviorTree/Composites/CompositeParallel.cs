using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Parallel")]
    [Category("Parallel")]
    
    [Image(typeof(IconCompositeParallel), ColorTheme.Type.TextLight)]
    [Description("Executes all of its children at the same time")]
    
    [Serializable]
    public class CompositeParallel : TComposite
    {
        private enum SuccessWhen
        {
            AtLeastOneSuccess,
            AtLeastOneFailure,
            AllSuccess,
            AllFailure,
            MoreSuccess,
            MoreFailure,
            AlwaysSuccess,
            AlwaysFailure
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private SuccessWhen m_SuccessWhen = SuccessWhen.AtLeastOneSuccess;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override Status Run(TNode node, Processor processor, Graph graph)
        {
            int numSuccess = 0;
            int numFailure = 0;
            
            if (node.Ports.Outputs.Length == 0) return Status.Success;
            TOutputPort output = node.Ports.Outputs[0];
            
            if (output.Connections.Length == 0) return Status.Success;

            foreach (Connection subNodeInputPort in output.Connections)
            {
                TNode subNode = graph.GetFromPortId(subNodeInputPort);
                Status outputStatus = subNode.Run(processor, graph);

                numSuccess += outputStatus == Status.Success ? 1 : 0;
                numFailure += outputStatus == Status.Failure ? 1 : 0;
            }
            
            if (numSuccess + numFailure >= output.Connections.Length)
            {
                return this.m_SuccessWhen switch
                {
                    SuccessWhen.AtLeastOneSuccess => numSuccess > 0
                        ? Status.Success
                        : Status.Failure,
                    
                    SuccessWhen.AtLeastOneFailure => numFailure > 0
                        ? Status.Success
                        : Status.Failure,
                    
                    SuccessWhen.AllSuccess => numFailure == 0 
                        ? Status.Success 
                        : Status.Failure,
                    
                    SuccessWhen.AllFailure => numSuccess == 0 
                        ? Status.Success 
                        : Status.Failure,
                    
                    SuccessWhen.MoreSuccess => numSuccess > numFailure
                        ? Status.Success
                        : Status.Failure,
                    
                    SuccessWhen.MoreFailure => numFailure > numSuccess
                        ? Status.Success
                        : Status.Failure,
                    
                    SuccessWhen.AlwaysSuccess => Status.Success,
                    SuccessWhen.AlwaysFailure => Status.Failure,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return Status.Running;
        }
    }
}