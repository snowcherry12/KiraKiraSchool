using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public abstract class TNodeUtilityBoard : TNode
    {
        [SerializeField] private Score m_Score = new Score();
        
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string ScoreTitle => this.m_Score.ToString();

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void CopyFrom(object source)
        {
            base.CopyFrom(source);
            if (source is not TNodeUtilityBoard sourceNode) return;
            
            this.m_Score = sourceNode.m_Score;
            this.m_Conditions = sourceNode.m_Conditions;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void Cancel(Processor processor, Graph graph)
        {
            if (this.GetStatus(processor) != Status.Running) return;
            processor.RuntimeData.SetStatus(this.Id, Status.Ready);
        }

        protected abstract IValueWithScore RequireData(Processor processor);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool CheckConditions(Args args) => this.m_Conditions.Check(args);

        public virtual float RecalculateScore(Processor processor, Graph graph)
        {
            IValueWithScore nodeData = this.RequireData(processor);
            float score = this.m_Score.Evaluate(processor.Args, graph as UtilityBoard);

            nodeData.Score = score;
            return nodeData.Score;
        }
    }
}