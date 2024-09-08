using System;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public struct QuestEntry
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private State m_State;
        [SerializeField] private bool m_IsTracking;

        // PROPERTIES: ----------------------------------------------------------------------------

        public State State
        {
            get => this.m_State;
            set => this.m_State = value;
        }

        public bool IsTracking
        {
            get => this.m_IsTracking;
            set => this.m_IsTracking = value;
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------
        
        public static QuestEntry NewActive(bool isTracking)
        {
            return new QuestEntry
            {
                m_State = State.Active,
                m_IsTracking = isTracking
            };
        }
        
        public static QuestEntry NewInactive(bool isTracking)
        {
            return new QuestEntry
            {
                m_State = State.Inactive,
                m_IsTracking = isTracking
            };
        }
        
        public static QuestEntry NewCompleted(bool isTracking)
        {
            return new QuestEntry
            {
                m_State = State.Completed,
                m_IsTracking = isTracking
            };
        }
        
        public static QuestEntry NewAbandoned(bool isTracking)
        {
            return new QuestEntry
            {
                m_State = State.Abandoned,
                m_IsTracking = isTracking
            };
        }
        
        public static QuestEntry NewFailed(bool isTracking)
        {
            return new QuestEntry
            {
                m_State = State.Failed,
                m_IsTracking = isTracking
            };
        }
    }
}