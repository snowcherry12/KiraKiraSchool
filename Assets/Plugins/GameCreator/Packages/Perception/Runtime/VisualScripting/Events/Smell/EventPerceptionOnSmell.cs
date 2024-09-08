using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Smell")]
    [Category("Perception/Smell/On Smell")]
    
    [Description("Executed when the Perception smells a Scent")]

    [Image(typeof(IconNose), ColorTheme.Type.Green)]
    [Keywords("Odor", "Smell", "Aroma", "Nose")]
    
    [Serializable]
    public class EventPerceptionOnSmell : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;
        
        [SerializeField]
        private CompareStringOrAny m_Scent = new CompareStringOrAny(
            true,
            GetStringId.Create("my-scent-id")
        );
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnAwake(Trigger trigger)
        {
            base.OnAwake(trigger);
            this.m_Args = new Args(this.Self);
        }

        protected override void OnUpdate(Trigger trigger)
        {
            base.OnUpdate(trigger);
            
            Perception perception = this.m_Perception.Get<Perception>(trigger);
            if (perception == null) return;

            if (this.m_Args.Target != perception.gameObject)
            {
                this.m_Args.ChangeTarget(perception.gameObject);
            }
            
            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            if (sensorSmell == null) return;

            float minIntensity = sensorSmell.MinIntensity;
            foreach (SensorSmell.Trace trace in sensorSmell.Traces)
            {
                if (minIntensity > trace.Score) continue;
                if (this.m_Scent.Match(trace.Tag, this.m_Args))
                {
                    _ = this.m_Trigger.Execute(this.m_Args);
                    return;
                }
            }
        }
    }
}