using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Set Attribute")]
    [Category("Stats/Set Attribute")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Sets a Attribute value")]

    [Parameter("To", "Where to store the Attribute asset")]
    [Parameter("Attribute", "The Attribute asset to store")]

    [Serializable]
    public class InstructionAttributesSetAttribute : Instruction
    {
        [SerializeField] private PropertySetAttribute m_To = SetAttributeNone.Create;
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override string Title => $"Set {this.m_To} = {this.m_Attribute}";
        
        protected override Task Run(Args args)
        {
            this.m_To.Set(this.m_Attribute.Get(args), args);
            return DefaultResult;
        }
    }
}