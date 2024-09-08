using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Awareness In Stage")]
    [Category("Perception/Awareness In Stage")]
    
    [Image(typeof(IconAwareness), ColorTheme.Type.Green)]
    [Description("Returns true if the Perception awareness of a target is in the specified stage")]
    
    [Keywords("Aware", "Alert", "Suspicious")]
    [Serializable]
    public class GetBoolAwarenessInStage : PropertyTypeGetBool
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private AwareMask m_Stage = AwareMask.Aware;

        public override bool Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return false;
            
            GameObject target = this.m_Target.Get(args);
            if (target == null) return false;
            
            Tracker tracker = perception.GetTracker(target);
            AwareStage currentStage = Tracker.GetStage(tracker.Awareness);

            return ((int) currentStage & (int) this.m_Stage) != 0;
        }

        public static PropertyGetBool Create => new PropertyGetBool(
            new GetBoolAwarenessInStage()
        );
        
        public override string String => $"{this.m_Perception}[{this.m_Target}] = {this.m_Stage}";

        public override bool EditorValue => false;
    }
}