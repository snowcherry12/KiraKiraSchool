using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("On Processor Finish")]
    [Category("Behavior/On Processor Finish")]
    [Description("Executed when the Processor finishes its current execution")]

    [Image(typeof(IconProcessor), ColorTheme.Type.Red)]
    [Keywords("AI", "Behavior Tree", "State Machine", "Utility", "Need", "Goal", "Plan", "GOAP")]

    [Serializable]
    public class EventProcessorFinish : VisualScripting.Event
    {
        [SerializeField] private PropertyGetGameObject m_Processor = GetGameObjectInstance.Create();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private Processor m_ProcessorCache;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            this.m_Args = new Args(trigger);

            this.RegisterProcessor();
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.m_ProcessorCache == null) return;
            this.m_ProcessorCache.EventFinish -= this.OnExecute;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RegisterProcessor()
        {
            if (this.m_ProcessorCache != null) this.m_ProcessorCache.EventFinish -= this.OnExecute;
            
            this.m_ProcessorCache = this.m_Processor.Get<Processor>(this.m_Args);
            if (this.m_ProcessorCache == null) return;
            
            this.m_Args.ChangeTarget(this.m_ProcessorCache);
            this.m_ProcessorCache.EventFinish += this.OnExecute;
        }
        
        private void OnExecute()
        {
            if (this.m_ProcessorCache == null) return;
            _ = this.m_Trigger.Execute(this.m_ProcessorCache.gameObject);
        }
    }
}