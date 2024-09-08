using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public struct TaskEntry
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private State m_State;
        [SerializeField] private double m_Value;

        // PROPERTIES: ----------------------------------------------------------------------------

        public State State
        {
            get => this.m_State;
            set => this.m_State = value;
        }

        public double Value
        {
            get => this.m_Value;
            set => this.m_Value = value;
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------
        
        public static TaskEntry NewActive(double value)
        {
            return new TaskEntry
            {
                m_State = State.Active,
                m_Value = value
            };
        }
        
        public static TaskEntry NewInactive(double value)
        {
            return new TaskEntry
            {
                m_State = State.Inactive,
                m_Value = value
            };
        }
        
        public static TaskEntry NewCompleted(double value)
        {
            return new TaskEntry
            {
                m_State = State.Completed,
                m_Value = value
            };
        }
        
        public static TaskEntry NewAbandoned(double value)
        {
            return new TaskEntry
            {
                m_State = State.Abandoned,
                m_Value = value
            };
        }
        
        public static TaskEntry NewFailed(double value)
        {
            return new TaskEntry
            {
                m_State = State.Failed,
                m_Value = value
            };
        }
    }
}