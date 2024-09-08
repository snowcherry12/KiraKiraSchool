using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    public static class RestoreUtils
    {
        private const string KEY_ACTION_PLAN = "restore-action-plan";
        private const string KEY_BEHAVIOR_TREE = "restore-behavior-tree";
        private const string KEY_STATE_MACHINE = "restore-state-machine";
        private const string KEY_UTILITY_BOARD = "restore-utility-board";
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        
        [Serializable]
        private class Paths
        {
            // EXPOSED MEMBERS: -------------------------------------------------------------------
            
            [SerializeField] private List<string> m_List = new List<string>();
            
            // PROPERTIES: ------------------------------------------------------------------------

            public List<string> List => this.m_List;
            
            // CONSTRUCTORS: ----------------------------------------------------------------------

            public Paths()
            { }

            public Paths(List<string> values)
            {
                this.m_List = values;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////

        public static string[] ActionPlan => Get(KEY_ACTION_PLAN).List.ToArray();
        public static string[] BehaviorTree => Get(KEY_BEHAVIOR_TREE).List.ToArray();
        public static string[] StateMachine => Get(KEY_STATE_MACHINE).List.ToArray();
        public static string[] UtilityBoard => Get(KEY_UTILITY_BOARD).List.ToArray();
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void UpdateActionPlan(List<string> paths)
        {
            Set(KEY_ACTION_PLAN, paths);
        }
        
        public static void UpdateBehaviorTree(List<string> paths)
        {
            Set(KEY_BEHAVIOR_TREE, paths);
        }
        
        public static void UpdateStateMachine(List<string> paths)
        {
            Set(KEY_STATE_MACHINE, paths);
        }
        
        public static void UpdateUtilityBoard(List<string> paths)
        {
            Set(KEY_UTILITY_BOARD, paths);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private static Paths Get(string key)
        {
            string json = SessionState.GetString(key, string.Empty);
            return JsonUtility.FromJson(json, typeof(Paths)) as Paths ?? new Paths();
        }
        
        private static void Set(string key, List<string> values)
        {
            Paths paths = new Paths(values);
            string json = JsonUtility.ToJson(paths);
            SessionState.SetString(key, json);
        }
    }
}