using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [Version(0, 1, 1)]
    
    [Title("Change AttributeUI Attribute")]
    [Category("Stats/UI/Change AttributeUI Attribute")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Changes the Attribute from a Attribute UI component")]

    [Parameter("Attribute UI", "The game object with the Attribute UI component")]
    [Parameter("Attribute", "The new Attribute asset")]

    [Serializable]
    public class InstructionStatsUIChangeAttributeUIAttribute : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_AttributeUI = GetGameObjectInstance.Create();
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override string Title =>
            $"Change {this.m_AttributeUI} Attribute to {this.m_Attribute}";
        
        protected override Task Run(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return DefaultResult;
            
            GameObject attributeUIGameObject = this.m_AttributeUI.Get(args);
            if (attributeUIGameObject == null) return DefaultResult;

            AttributeUI attributeUI = attributeUIGameObject.Get<AttributeUI>();
            if (attributeUI == null) return DefaultResult;
            
            attributeUI.Attribute = attribute;
            return DefaultResult;
        }
    }
}