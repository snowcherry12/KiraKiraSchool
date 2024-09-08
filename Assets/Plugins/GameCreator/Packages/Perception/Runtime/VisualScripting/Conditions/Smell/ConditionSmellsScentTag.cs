using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Smells Scent Tag")]
    [Description("Checks whether the Perception component is smelling a Scent Tag")]

    [Category("Perception/Smell/Smells Scent Tag")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Scent Tag", "The Scent Tag to check")]
    [Parameter("Value", "The comparison to the scent value")]

    [Keywords("Aroma", "Scent", "Smell", "Sniff", "Nose", "Trace")]
    [Image(typeof(IconScent), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class ConditionSmellsScentTag : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetString m_ScentTag = GetStringId.Create("my-scent-tag");

        [SerializeField]
        private CompareDouble m_Value = new CompareDouble();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => 
            $"{this.m_Perception}[{this.m_ScentTag}] Scent {this.m_Value}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return false;

            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            if (sensorSmell == null) return false;

            float intensity = sensorSmell.GetIntensity(this.m_ScentTag.Get(args));
            return this.m_Value.Match(intensity, args);
        }
    }
}
