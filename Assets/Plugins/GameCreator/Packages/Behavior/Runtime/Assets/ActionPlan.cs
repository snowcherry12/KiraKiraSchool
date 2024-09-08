using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [CreateAssetMenu(
        fileName = "My Action Plan", 
        menuName = "Game Creator/Behavior/Action Plan"
    )]
    
    [Icon(EditorPaths.PACKAGES + "Behavior/Editor/Gizmos/GizmoActionPlan.png")]
    
    [Serializable]
    public class ActionPlan : Graph
    {
        public const int INDEX_ROOT = 0;
        
        private static readonly Vector2 ROOT_POSITION = new Vector2(0f, 50f);
        private static readonly Vector2 TASK_POSITION = new Vector2(0f, -50f);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Thoughts m_Thoughts = new Thoughts();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        internal Thoughts Thoughts => this.m_Thoughts;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void Reset()
        {
            this.m_Nodes = new TNode[]
            {
                new NodeActionPlanRoot { Position = ROOT_POSITION },
                new NodeActionPlanTaskInstructions { Position = TASK_POSITION }
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override Status Run(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return Status.Success;
            
            NodeActionPlanRoot nodeRoot = this.m_Nodes[INDEX_ROOT] as NodeActionPlanRoot;
            return nodeRoot?.Run(processor, this) ?? Status.Success;
        }

        public override void Abort(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return;
            
            NodeActionPlanRoot nodeRoot = this.m_Nodes[INDEX_ROOT] as NodeActionPlanRoot;
            nodeRoot?.Abort(processor, this);
        }

        public void AddGoal(Goal goal, Processor processor)
        {
            if (this.m_Nodes.Length == 0) return;
            
            TNode nodeRoot = this.m_Nodes[INDEX_ROOT];
            ValueActionPlanRoot value = processor.RuntimeData.GetValue<ValueActionPlanRoot>(nodeRoot.Id);

            if (value == null)
            {
                value = new ValueActionPlanRoot(this);
                processor.RuntimeData.SetValue(nodeRoot.Id, value);
            }

            value.AddGoal(goal);
        }

        public void RemoveGoal(Goal goal, Processor processor)
        {
            if (this.m_Nodes.Length == 0) return;
            
            TNode nodeRoot = this.m_Nodes[INDEX_ROOT];
            if (nodeRoot == null) return;

            ValueActionPlanRoot value = processor.RuntimeData.GetValue<ValueActionPlanRoot>(nodeRoot.Id);
            value?.RemoveGoal(goal);
        }
    }
}