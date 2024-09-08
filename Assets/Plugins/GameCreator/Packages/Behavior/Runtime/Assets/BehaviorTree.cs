using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [CreateAssetMenu(
        fileName = "My Behavior Tree", 
        menuName = "Game Creator/Behavior/Behavior Tree"
    )]
    
    [Icon(EditorPaths.PACKAGES + "Behavior/Editor/Gizmos/GizmoBehaviorTree.png")]
    
    [Serializable]
    public class BehaviorTree : Graph
    {
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void Reset()
        {
            this.m_Nodes = new TNode[]
            {
                new NodeBehaviorTreeEntry()
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override Status Run(Processor processor)
        {
            return this.m_Nodes.Length != 0 
                ? this.m_Nodes[0].Run(processor, this)
                : Status.Success;
        }

        public override void Abort(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return;
            this.m_Nodes[0].Abort(processor, this);
        }
    }
}