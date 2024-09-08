using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Change Attribute")]
    [Category("Stats/Change Attribute")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Changes the current Attribute value of a game object's Traits component")]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("Attribute", "The Attribute type that changes its value")]
    [Parameter("Change", "The value changed")]
    
    [Keywords("Health", "HP", "Mana", "MP", "Stamina")]

    [Serializable]
    public class InstructionStatsChangeAttribute : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();
        
        [SerializeField] private ChangeDecimal m_Change = new ChangeDecimal(100f);
        
        public override string Title => $"{this.m_Target}[{this.m_Attribute}] {this.m_Change}";
        
        protected override Task Run(Args args)
        {
            GameObject target = this.m_Target.Get(args);
            if (target == null) return DefaultResult;

            Traits traits = target.Get<Traits>();
            if (traits == null) return DefaultResult;

            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return DefaultResult;
            
            RuntimeAttributeData runtimeAttribute = traits.RuntimeAttributes.Get(attribute.ID);
            if (runtimeAttribute == null) return DefaultResult;

            runtimeAttribute.Value = (float) this.m_Change.Get(runtimeAttribute.Value, args);
            return DefaultResult;
        }
    }
}