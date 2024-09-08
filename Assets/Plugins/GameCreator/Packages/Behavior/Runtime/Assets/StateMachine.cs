using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [CreateAssetMenu(
        fileName = "My State Machine", 
        menuName = "Game Creator/Behavior/State Machine"
    )]
    
    [Icon(EditorPaths.PACKAGES + "Behavior/Editor/Gizmos/GizmoStateMachine.png")]
    
    [Serializable]
    public class StateMachine : Graph
    {
        public const int INDEX_ENTER = 0;
        public const int INDEX_EXIT = 1;

        private static readonly Vector2 ENTER_POSITION = new Vector2(-100f, 0f);
        private static readonly Vector2 EXIT_POSITION = new Vector2(100f, 0f);
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void Reset()
        {
            this.m_Nodes = new TNode[]
            {
                new NodeStateMachineEnter { Position = ENTER_POSITION },
                new NodeStateMachineExit  { Position = EXIT_POSITION  },
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override Status Run(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return Status.Success;
            
            TNode nodeEnter = this.m_Nodes[INDEX_ENTER];
            TNode nodeExit = this.m_Nodes[INDEX_EXIT];
            
            ValueStateMachineEnter value = nodeEnter.GetValue<ValueStateMachineEnter>(processor);
            if (value == null || value.CurrentNodeId == IdString.EMPTY)
            {
                nodeEnter.Run(processor, this);
            }
            else
            {
                TNode currentNode = this.GetFromNodeId(value.CurrentNodeId);
                currentNode.Run(processor, this);
            }

            return nodeExit.GetStatus(processor) == Status.Success
                ? Status.Success
                : Status.Running;
        }

        public override void Abort(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return;
            TNode nodeEnter = this.m_Nodes[INDEX_ENTER];
            
            ValueStateMachineEnter value = nodeEnter.GetValue<ValueStateMachineEnter>(processor);
            if (value == null) return;
            
            TNode currentNode = this.GetFromNodeId(value.CurrentNodeId);
            currentNode.Abort(processor, this);
        }
    }
}