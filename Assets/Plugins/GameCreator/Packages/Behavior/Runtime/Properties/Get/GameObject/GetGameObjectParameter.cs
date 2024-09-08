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
    [Description("Returns the Game Object value from a Processor's Parameter")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectParameter : PropertyTypeGetGameObject
    {
        [SerializeField] private PropertyGetGameObject m_Processor = GetGameObjectSelf.Create();
        [SerializeField] private IdString m_Name;

        public override GameObject Get(Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            
            if (processor == null) return default;
            return processor.RuntimeData.GetParameter(this.m_Name.String) as GameObject;
        }

        public override string String => $"{this.m_Processor}[{m_Name}]";
    }
}