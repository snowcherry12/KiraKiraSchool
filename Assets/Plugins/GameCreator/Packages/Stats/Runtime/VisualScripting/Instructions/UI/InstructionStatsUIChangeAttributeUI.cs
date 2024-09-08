using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats.UnityUI
{
    [Version(0, 1, 1)]
    
    [Title("Change AttributeUI Target")]
    [Category("Stats/UI/Change AttributeUI Target")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Changes the targeted game object of an Attribute UI component")]

    [Parameter("Attribute UI", "The game object with the Attribute UI component")]
    [Parameter("Target", "The new targeted game object with a Traits component")]

    [Serializable]
    public class InstructionStatsUIChangeAttributeUI : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_AttributeUI = GetGameObjectInstance.Create();

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        public override string Title => $"Change {this.m_AttributeUI} target to {this.m_Target}";
        
        protected override Task Run(Args args)
        {
            GameObject attributeUIGameObject = this.m_AttributeUI.Get(args);
            if (attributeUIGameObject == null) return DefaultResult;

            AttributeUI attributeUI = attributeUIGameObject.Get<AttributeUI>();
            if (attributeUI == null) return DefaultResult;

            attributeUI.Target = this.m_Target.Get(args);
            return DefaultResult;
        }
    }
}