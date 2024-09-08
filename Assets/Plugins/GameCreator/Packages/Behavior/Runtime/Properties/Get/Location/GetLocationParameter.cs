using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Parameter")]
    [Category("Behavior/Parameter")]

    [Keywords("Processor", "Behavior Tree", "State Machine", "Finite", "FSM", "GOAP", "Goal")]
    [Keywords("Blackboard", "Value")]
    
    [Image(typeof(IconProcessor), ColorTheme.Type.Blue)]
    [Description("Returns the Game Object, Marker or Vector3 location value from a Processor's Parameter")]

    [Serializable]
    public class GetLocationParameter : PropertyTypeGetLocation
    {
        [SerializeField] private PropertyGetGameObject m_Processor = GetGameObjectSelf.Create();
        [SerializeField] private IdString m_Name; 

        public override Location Get(Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            
            if (processor == null) return Location.None;
            object value = processor.RuntimeData.GetParameter(this.m_Name.String);
            
            switch (value)
            {
                case null: return Location.None;
                case GameObject valueGameObject:
                    Marker marker = valueGameObject.Get<Marker>();
                    return marker != null 
                        ? new Location(marker) 
                        : new Location(valueGameObject.transform.position, valueGameObject.transform.rotation);
                case Vector3 valueVector3: return new Location(valueVector3);
                default: return Location.None;
            }
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationParameter()
        );

        public override string String => $"{this.m_Processor}[{this.m_Name}]";
    }
}