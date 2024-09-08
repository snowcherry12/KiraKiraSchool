using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [CreateAssetMenu(
        fileName = "Formula", 
        menuName = "Game Creator/Stats/Formula",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoFormula.png")]
    
    public class Formula : ScriptableObject
    {
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode()
        {
            LastFormulaResult = 0f;
        }
        
        #endif
        
        private static readonly Input[] Inputs = 
        {
            new Input(@"source\.base\[[a-zA-Z0-9_\-]+\]", BaseSourceName),
            new Input(@"source\.stat\[[a-zA-Z0-9_\-]+\]", StatSourceName),
            new Input(@"source\.attr\[[a-zA-Z0-9_\-]+\]", AttrSourceName),
            new Input(@"target\.base\[[a-zA-Z0-9_\-]+\]", BaseTargetName),
            new Input(@"target\.stat\[[a-zA-Z0-9_\-]+\]", StatTargetName),
            new Input(@"target\.attr\[[a-zA-Z0-9_\-]+\]", AttrTargetName),
            new Input(@"source\.var\[[a-zA-Z0-9_\-]+\]", VariableSourceName),
            new Input(@"target\.var\[[a-zA-Z0-9_\-]+\]", VariableTargetName)
        };
        
        private static readonly Regex RX_EXTRACT_NAME = new Regex(@"\[(?:\[??([^\[]*?)\])");
        
        // STATIC MEMBERS: ------------------------------------------------------------------------
        
        [field: NonSerialized] public static double LastFormulaResult { get; private set; }

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private string m_Formula;
        [SerializeField] private Table m_Table;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private int m_FormulaHash;
        [NonSerialized] private Expression m_Expression;
        [NonSerialized] private Domain m_Domain;
        
        [NonSerialized] private Parameter[] m_Parameters;
        [NonSerialized] private double[] m_Input;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool Exists => !string.IsNullOrEmpty(this.m_Formula);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public double Calculate(GameObject source, GameObject target)
        {
            #if UNITY_EDITOR
            
            if (this.m_Formula.GetHashCode() != this.m_FormulaHash)
            {
                this.Initialize();
            }
            
            #else
            
            if (this.m_Expression == null)
            {
                this.Initialize();
            }
            
            #endif

            this.m_Domain.Set(this.m_Table, source, target);
            this.SetInputs();

            LastFormulaResult = this.m_Expression.Evaluate(this.m_Domain, this.m_Input, out double input) 
                ? input 
                : default;

            return LastFormulaResult;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnValidate()
        {
            this.m_FormulaHash = default;
        }
        
        private void Initialize()
        {
            StringBuilder formula = new StringBuilder(this.m_Formula);

            List<Parameter> variables = new List<Parameter>();
            while (FindParameter(formula.ToString(), out Match match, out int index))
            {
                formula = formula
                    .Remove(match.Index, match.Length)
                    .Insert(match.Index, '#');
                
                Parameter variable = new Parameter(
                    ClauseName(match.Value),
                    Inputs[index].Function
                );
                
                variables.Add(variable);
            }

            this.m_Parameters = variables.ToArray();
            this.m_Input = new double[this.m_Parameters.Length];
            
            this.m_Expression = new Expression(formula.ToString());
            this.m_Domain = new Domain();
            
            this.m_FormulaHash = this.m_Formula.GetHashCode();
        }

        private static bool FindParameter(string formula, out Match match, out int index)
        {
            match = null;
            index = -1;
            
            int minIndex = int.MaxValue;
            
            for (int i = 0; i < Inputs.Length; ++i)
            {
                Match candidate = Inputs[i].Match(formula);
                if (candidate.Success && candidate.Index < minIndex)
                {
                    index = i;
                    match = candidate;
                    minIndex = candidate.Index;
                }
            }

            return minIndex < int.MaxValue;
        }

        private void SetInputs()
        {
            for (int i = 0; i < this.m_Parameters.Length; ++i)
            {
                this.m_Input[i] = this.m_Parameters[i].Function.Invoke(
                    this.m_Domain,
                    this.m_Parameters[i].Name
                );
            }
        }

        private static string ClauseName(string clause)
        {
            Match match = RX_EXTRACT_NAME.Match(clause);
            return match.Success && match.Groups.Count == 2
                ? match.Groups[1].Value
                : string.Empty;
        }

        // FUNCTIONS: -----------------------------------------------------------------------------

        private static double BaseSourceName(Domain data, string name)
        {
            return data.Source != null
                ? data.Source.Get<Traits>().RuntimeStats.Get(name).Base
                : default;
        }

        private static double StatSourceName(Domain data, string name)
        {
            return data.Source != null
                ? data.Source.Get<Traits>().RuntimeStats.Get(name).Value
                : default;
        }

        private static double AttrSourceName(Domain data, string name)
        {
            return data.Source != null
                ? data.Source.Get<Traits>().RuntimeAttributes.Get(name).Value
                : default;
        }

        private static double BaseTargetName(Domain data, string name)
        {
            return data.Target != null
                ? data.Target.Get<Traits>().RuntimeStats.Get(name).Base
                : default;
        }

        private static double StatTargetName(Domain data, string name)
        {
            return data.Target != null
                ? data.Target.Get<Traits>().RuntimeStats.Get(name).Value
                : default;
        }

        private static double AttrTargetName(Domain data, string name)
        {
            return data.Target != null
                ? data.Target.Get<Traits>().RuntimeAttributes.Get(name).Value
                : default;
        }

        private static double VariableSourceName(Domain data, string name)
        {
            if (data.Source == null) return default;
            LocalNameVariables variables = data.Source.gameObject.Get<LocalNameVariables>();
            if (variables == null) return default;
            object value = variables.Get(name);
            return Convert.ToSingle(value);
        }

        private static double VariableTargetName(Domain data, string name)
        {
            if (data.Target == null) return default;
            LocalNameVariables variables = data.Target.gameObject.Get<LocalNameVariables>();
            if (variables == null) return default;
            object value = variables.Get(name);
            return Convert.ToSingle(value);
        }
    }
}