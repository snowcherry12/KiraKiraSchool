using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Can Smell Scent")]
    [Description("Checks whether the Perception component can smell a new Scent stimulus")]

    [Category("Perception/Smell/Can Smell Scent")]
    
    [Parameter("Perception", "The Perception component")]
    [Parameter("Position", "The position of the Scent stimulus")]
    [Parameter("Radius", "The radius of the Scent stimulus")]
    [Parameter("Intensity", "The intensity of the Scent stimulus")]

    [Keywords("Aroma", "Scent", "Smell", "Sniff", "Nose", "Trace")]
    [Image(typeof(IconScent), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionCanSmellScent : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField] private PropertyGetPosition m_Position = GetPositionCharactersPlayer.Create;
        [SerializeField] private PropertyGetDecimal m_Radius = GetDecimalDecimal.Create(10f);
        [SerializeField] private PropertyGetDecimal m_Intensity = GetDecimalConstantOne.Create;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary =>
            $"{this.m_Perception} smell Scent at {this.m_Position}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return false;

            SensorSmell sensorSmell = perception.GetSensor<SensorSmell>();
            if (sensorSmell == null) return false;

            StimulusScent stimulus = new StimulusScent(
                string.Empty,
                this.m_Position.Get(args),
                (float) this.m_Radius.Get(args),
                (float) this.m_Intensity.Get(args)
            );
            
            return sensorSmell.CanSmell(stimulus);
        }
    }
}
