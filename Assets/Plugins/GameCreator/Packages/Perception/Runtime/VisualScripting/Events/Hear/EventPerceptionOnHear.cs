using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Hear")]
    [Category("Perception/Hear/On Hear")]
    
    [Description("Executed when the Perception hears a Noise")]

    [Image(typeof(IconEar), ColorTheme.Type.Green)]
    [Keywords("Sound", "Noise", "Distract", "Alert", "Aural", "Hear")]
    
    [Serializable]
    public class EventPerceptionOnHear : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;
        
        [SerializeField]
        private CompareStringOrAny m_Noise = new CompareStringOrAny(
            true,
            GetStringId.Create("my-noise-id")
        );
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;
        [NonSerialized] private GameObject m_Source;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnAwake(Trigger trigger)
        {
            base.OnAwake(trigger);
            this.m_Args = new Args(this.Self);
        }

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            Perception perception = this.m_Perception.Get<Perception>(trigger);
            if (perception == null) return;
            
            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            if (sensorHear == null) return;

            this.m_Source = perception.gameObject;
            this.m_Args.ChangeTarget(perception.gameObject);
            
            sensorHear.EventHearNoise -= this.OnHearNoise;
            sensorHear.EventHearNoise += this.OnHearNoise;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;

            Perception perception = this.m_Source != null ? this.m_Source.Get<Perception>() : null;
            if (perception == null) return;
            
            SensorHear sensorHear = perception.GetSensor<SensorHear>();
            if (sensorHear == null) return;

            sensorHear.EventHearNoise -= this.OnHearNoise;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnHearNoise(string noiseTag)
        {
            if (!this.m_Noise.Match(noiseTag, this.m_Args)) return;
            _ = this.m_Trigger.Execute(this.m_Args);
        }
    }
}