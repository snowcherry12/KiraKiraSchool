using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [CreateAssetMenu(
        fileName = "My Utility Board", 
        menuName = "Game Creator/Behavior/Utility Board"
    )]
    
    [Icon(EditorPaths.PACKAGES + "Behavior/Editor/Gizmos/GizmoUtilityBoard.png")]
    
    [Serializable]
    public class UtilityBoard : Graph
    {
        private const int INDEX_ROOT = 0;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetDecimal m_Minimum = new PropertyGetDecimal(0f);
        [SerializeField] private PropertyGetDecimal m_Maximum = new PropertyGetDecimal(1f);
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void Reset()
        {
            this.m_Nodes = new TNode[]
            {
                new NodeUtilityBoardRoot(),
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public float GetMinimum(Args args) => (float) this.m_Minimum.Get(args);
        public float GetMaximum(Args args) => (float) this.m_Maximum.Get(args);
        
        public override Status Run(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return Status.Success;
            
            TNode root = this.m_Nodes[INDEX_ROOT];
            ValueUtilityBoardRoot rootData = root.GetValue<ValueUtilityBoardRoot>(processor);
            if (rootData == null)
            {
                rootData = new ValueUtilityBoardRoot();
                processor.RuntimeData.SetValue(root.Id, rootData);
            }
            
            IdString runningNodeId = rootData.RunningNodeId;

            float maxScore = float.MinValue;
            TNodeUtilityBoard maxNode = null;

            for (int i = 1; i < this.m_Nodes.Length; i++)
            {
                if (this.m_Nodes[i] is not TNodeUtilityBoard nodeUtilityBoard) continue;
                
                float score = nodeUtilityBoard.RecalculateScore(processor, this);
                if (score <= maxScore) continue;

                if (nodeUtilityBoard.CheckConditions(processor.Args))
                {
                    maxScore = score;
                    maxNode = nodeUtilityBoard;
                }
            }

            if (maxNode == null) return Status.Success;

            if (runningNodeId != IdString.EMPTY && maxNode.Id != runningNodeId)
            {
                TNode cancelNode = this.GetFromNodeId(runningNodeId);
                cancelNode?.Abort(processor, this);
            }

            rootData.RunningNodeId = maxNode.Id;
            return maxNode.Run(processor, this);
        }

        public override void Abort(Processor processor)
        {
            if (this.m_Nodes.Length == 0) return;
            
            TNode root = this.m_Nodes[INDEX_ROOT];
            ValueUtilityBoardRoot rootData = root.GetValue<ValueUtilityBoardRoot>(processor);
            
            if (rootData.RunningNodeId == IdString.EMPTY) return;
            TNode currentNode = this.GetFromNodeId(rootData.RunningNodeId);
            
            currentNode.Abort(processor, this);
            rootData.RunningNodeId = IdString.EMPTY;
        }
    }
}