using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class NodeActionPlanRoot : TNodeActionPlan
    {
        public const string TYPE_ID = "action-plan:root";

        // PROPERTIES: ----------------------------------------------------------------------------

        public override PropertyName TypeId => TYPE_ID;

        // PUBLIC GETTERS: ------------------------------------------------------------------------

        public override float GetCost(Args args) => 0f;

        public override Beliefs Beliefs => null;
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override Status Update(Processor processor, Graph graph)
        {
            Status status = this.GetStatus(processor);
            ValueActionPlanRoot data = this.GetValue<ValueActionPlanRoot>(processor);

            if (data == null)
            {
                data = new ValueActionPlanRoot(graph);
                processor.RuntimeData.SetValue(this.Id, data);
            }

            if (status == Status.Success || status == Status.Failure)
            {
                data.Restart();
            }
            
            if (status != Status.Running) data.Plan(processor, graph);

            if (data.CurrentPlan.Exists)
            {
                Plan plan = data.CurrentPlan;
                foreach (IdString nodeId in plan.Sequence)
                {
                    TNode node = graph.GetFromNodeId(nodeId);
                    switch (processor.RuntimeData.GetStatus(nodeId))
                    {
                        case Status.Ready:
                        case Status.Running:
                            Status result = node.Run(processor, graph);
                            if (result == Status.Running) return Status.Running;
                            if (result == Status.Failure) return Status.Failure;
                            break;
                        
                        case Status.Success:
                            continue;

                        case Status.Failure:
                            return Status.Failure;
                        
                        default: throw new ArgumentOutOfRangeException();
                    }
                }

                return Status.Success;
            }

            return Status.Failure;
        }

        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => $"Root:{this.Id}";
    }
}