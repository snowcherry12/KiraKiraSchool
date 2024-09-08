using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Parameter")]
    [Category("Behavior/Parameter")]
    [Keywords("Processor", "Behavior Tree", "State Machine", "Finite", "FSM", "GOAP", "Goal")]
    [Keywords("Blackboard", "Value")]
    
    [Image(typeof(IconProcessor), ColorTheme.Type.Blue)]
    [Description("Returns the Euler rotation Vector3 value from a Processor's Parameter")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetRotationParameter : PropertyTypeGetRotation
    {
        [SerializeField] private PropertyGetGameObject m_Processor = GetGameObjectSelf.Create();
        [SerializeField] private IdString m_Name;

        public override Quaternion Get(Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            if (processor == null) return default;
            
            object value = processor.RuntimeData.GetParameter(this.m_Name.String);
            Vector3 angles = value != null ? (Vector3) value : default;
            
            return Quaternion.Euler(angles);
        }

        public override string String => $"{this.m_Processor}[{m_Name}]";
    }
}