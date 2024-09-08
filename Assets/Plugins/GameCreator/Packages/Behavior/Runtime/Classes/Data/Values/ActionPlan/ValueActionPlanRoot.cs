using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    public class ValueActionPlanRoot : IValue
    {
        [NonSerialized] private readonly Dictionary<PropertyName, Goal> m_Goals;
        [NonSerialized] private readonly HashSet<IdString> m_Tasks;
        [NonSerialized] private readonly List<Step> m_EndingSteps;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public Plan CurrentPlan { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ValueActionPlanRoot(Graph graph)
        {
            this.m_Goals = new Dictionary<PropertyName, Goal>();
            this.m_Tasks = new HashSet<IdString>();
            this.m_EndingSteps = new List<Step>();
            
            foreach (TNode node in graph.Nodes)
            {
                if (node is not NodeActionPlanTaskInstructions && node is not NodeActionPlanTaskSubgraph) continue;
                this.m_Tasks.Add(node.Id);
            }
            
            this.CurrentPlan = new Plan();
        }
        
        // GOAL METHODS: --------------------------------------------------------------------------

        public void AddGoal(Goal goal)
        {
            this.RemoveGoal(goal);
            this.m_Goals.Add(goal.Name.String, goal);
        }

        public void RemoveGoal(Goal goal)
        {
            this.m_Goals.Remove(goal.Name.String);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Plan(Processor processor, Graph graph)
        {
            this.CurrentPlan.Reset();
            
            State state = ((ActionPlan) graph).Thoughts.Meditate(processor);
            
            foreach (KeyValuePair<PropertyName, Goal> entry in this.m_Goals)
            {
                Plan plan = this.Plan(processor, graph, entry.Value, state);
                plan.GenerateWeight(this.m_Goals.Values);
                
                if (this.CurrentPlan.Exists && this.CurrentPlan.Weight > plan.Weight) continue;
                this.CurrentPlan = plan;
            }
        }

        public void Restart()
        {
            this.CurrentPlan.Reset();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private Plan Plan(Processor processor, Graph graph, Goal goal, State state)
        {
            List<IdString> candidates = new List<IdString>(this.m_Tasks);
            this.m_EndingSteps.Clear();
            
            Step firstStep = new Step(IdString.EMPTY, null, 0f, state);
            bool success = this.SearchPath(firstStep, candidates, goal, processor, graph);

            if (!success)
            {
                return Behavior.Plan.NONE;
            }
            
            Step minEndingStep = null;
            foreach (Step endingStep in this.m_EndingSteps) 
            {
                if (minEndingStep == null) 
                {
                    minEndingStep = endingStep;
                    continue;
                }

                if (minEndingStep.Cost > endingStep.Cost) 
                {
                    minEndingStep = endingStep;
                }
            }

            if (minEndingStep == null) return Behavior.Plan.NONE;
            
            Plan plan = new Plan();
            Step step = minEndingStep;

            while (step != null)
            {
                if (step.Id != IdString.EMPTY) plan.Insert(step.Id);
                step = step.Parent;
            }
            
            plan.Cost = minEndingStep.Cost;
            plan.CompleteState = minEndingStep.ResolveState;
            
            return plan;
        }

        private bool SearchPath(Step parent, List<IdString> candidateIds,
            Goal goal, Processor processor, Graph graph)
        {
            bool hasPath = false;
            foreach (IdString candidateId in candidateIds)
            {
                TNodeActionPlanTask candidate = (TNodeActionPlanTask) graph.GetFromNodeId(candidateId);
                if (!candidate.ResolveBy(parent.ResolveState, graph)) continue;
                
                State state = parent.ResolveState.Copy();
                state.Apply(candidate.GetPostConditions(graph));
                
                Step step = new Step(
                    candidateId, parent, 
                    parent.Cost + candidate.GetCost(processor.Args),
                    state
                );
                
                if (state.Get(goal.Name.String))
                {
                    this.m_EndingSteps.Add(step);
                    hasPath = true;
                }
                else
                {
                    List<IdString> subsetCandidates = CopyTasksSubset(candidateIds, candidateId);
                    if (SearchPath(step, subsetCandidates, goal, processor, graph)) hasPath = true;
                }
            }

            return hasPath;
        }

        // PRIVATE STATIC METHODS: ----------------------------------------------------------------
        
        private static List<IdString> CopyTasksSubset(List<IdString> taskIds, IdString removeTaskId)
        {
            List<IdString> result = new List<IdString>(taskIds.Count - 1);
            foreach (IdString taskId in taskIds)
            {
                if (taskId == removeTaskId) continue;
                result.Add(taskId);
            }
        
            return result;
        }
    }
}