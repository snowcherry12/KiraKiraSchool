using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Can See")]
    [Description("Returns true if object can be seen by the Perception component")]

    [Category("Perception/See/Can See")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Target", "The Game Object checked")]

    [Keywords("See", "Sight", "Vision", "Detect")]
    [Image(typeof(IconEye), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionPerceptionCanSee : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Perception} can see {this.m_Target}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            GameObject target = this.m_Target.Get(args);
            
            if (perception == null) return false;
            if (target == null) return false;

            SensorSee sensorSee = perception.GetSensor<SensorSee>();
            return sensorSee != null && sensorSee.CanSee(target);
        }
    }
}
