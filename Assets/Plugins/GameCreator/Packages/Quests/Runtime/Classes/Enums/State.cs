using System;

namespace GameCreator.Runtime.Quests
{
    public enum State
    {
        Inactive  = 0,
        Active    = 1,
        Completed = 2,
        Abandoned = 3,
        Failed    = 4,
    }
    
    [Flags]
    public enum StateFlags
    {
        Inactive  = 1, // 0b00001
        Active    = 2, // 0b00010
        Completed = 4, // 0b00100
        Abandoned = 8, // 0b01000
        Failed    = 16 // 0b10000 
    }
    
    public static class StateExtensions
    {
        public static bool IsActive(this State state)
        {
            return state == State.Active;
        }
        
        public static bool IsInactive(this State state)
        {
            return state == State.Inactive;
        }
        
        public static bool IsCompleted(this State state)
        {
            return state == State.Completed;
        }
        
        public static bool IsAbandoned(this State state)
        {
            return state == State.Abandoned;
        }
        
        public static bool IsFailed(this State state)
        {
            return state == State.Failed;
        }
        
        public static bool IsFinished(this State state)
        {
            return IsCompleted(state) || IsAbandoned(state) || IsFailed(state);
        }
    }
}