using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Compare Awareness")]
    [Description("Compares the Awareness value with another value")]

    [Category("Perception/Compare Awareness")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Target", "The Game Object checked")]
    [Parameter("Value", "The comparison to the Awareness value")]

    [Keywords("Awareness", "Track", "See", "Alert", "Suspicious", "Aware")]
    [Image(typeof(IconAwareness), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionPerceptionAwareness : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField]
        private CompareDouble m_Value = new CompareDouble();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => 
            $"{this.m_Perception}[{this.m_Target}] Awareness {this.m_Value}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            GameObject target = this.m_Target.Get(args);
            
            if (perception == null) return false;
            if (target == null) return false;

            Tracker tracker = perception.GetTracker(target);
            return tracker != null && this.m_Value.Match(tracker.Awareness, args);
        }
    }
}
