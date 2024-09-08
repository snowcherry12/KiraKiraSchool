using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("In Awareness Stage")]
    [Description("Returns true if the awareness of a target is in any of the specified stages")]

    [Category("Perception/In Awareness Stage")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Target", "The Game Object checked")]
    [Parameter("Stage", "The stage(s) to check")]

    [Keywords("Awareness", "Track", "See", "Alert", "Suspicious", "Aware")]
    [Image(typeof(IconAwareness), ColorTheme.Type.Blue, typeof(OverlayDot))]
    
    [Serializable]
    public class ConditionPerceptionAwareStage : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private AwareMask m_Stage = AwareMask.Alert;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Perception}[{this.m_Target}] is {this.m_Stage}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            GameObject target = this.m_Target.Get(args);
            
            if (perception == null) return false;
            if (target == null) return false;

            Tracker tracker = perception.GetTracker(target);
            if (tracker == null) return false;
            
            AwareStage stage = Tracker.GetStage(tracker.Awareness);
            return ((int) stage & (int) this.m_Stage) != 0;
        }
    }
}
