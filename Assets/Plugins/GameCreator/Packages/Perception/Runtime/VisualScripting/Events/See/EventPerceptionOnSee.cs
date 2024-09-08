using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On See")]
    [Category("Perception/See/On See")]
    
    [Description("Executed when the Perception sees the specified (tracked) game object")]

    [Image(typeof(IconEye), ColorTheme.Type.Green)]
    [Keywords("Track", "Vision", "Sight")]
    [Example("This Event will only execute on game objects that are being tracked")]
    
    [Serializable]
    public class EventPerceptionOnSee : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private CompareGameObjectOrAny m_Target = new CompareGameObjectOrAny(
            true,
            GetGameObjectPlayer.Create()
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
            
            SensorSee sensorSee = perception.GetSensor<SensorSee>();
            if (sensorSee == null) return;
            
            this.m_Source = perception.gameObject;
            this.m_Args.ChangeTarget(perception.gameObject);
            
            sensorSee.EventSee -= this.OnSee;
            sensorSee.EventSee += this.OnSee;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;

            Perception perception = this.m_Source != null ? this.m_Source.Get<Perception>() : null;
            if (perception == null) return;
            
            SensorSee sensorSee = perception.GetSensor<SensorSee>();
            if (sensorSee == null) return;
            
            sensorSee.EventSee -= this.OnSee;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnSee(GameObject target)
        {
            if (!this.m_Target.Match(target, this.m_Args)) return;
            _ = this.m_Trigger.Execute(this.m_Args);
        }
    }
}