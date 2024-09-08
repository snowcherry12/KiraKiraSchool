using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Parameter")]
    [Category("Behavior/Parameter")]
    [Keywords("Processor", "Behavior", "Finite", "State", "Machine", "FSM", "Goal", "GOAP")]
    [Keywords("Blackboard", "Value")]
    
    [Image(typeof(IconProcessor), ColorTheme.Type.Blue)]
    [Description("Sets the Texture value of a Processor's Parameter")]

    [Serializable] [HideLabelsInEditor]
    public class SetTextureParameter : PropertyTypeSetTexture
    {
        [SerializeField] private PropertyGetGameObject m_Processor = GetGameObjectSelf.Create();
        [SerializeField] private IdString m_Name;

        public override void Set(Texture value, Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            if (processor == null) return;
            processor.RuntimeData.SetParameter(this.m_Name.String, value);
        }

        public override Texture Get(Args args)
        {
            Processor processor = this.m_Processor.Get<Processor>(args);
            if (processor == null) return default;
            return processor.RuntimeData.GetParameter(this.m_Name.String) as Texture;
        }

        public override string String => $"{this.m_Processor}[{m_Name}]";
    }
}