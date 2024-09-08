using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Version(0, 0, 1)]
    
    [Title("Set Shield")]
    [Description("Sets the Shield value")]

    [Category("Melee/Defense/Set Shield")]
    
    [Parameter("To", "The location where to store the Shield")]
    [Parameter("Shield", "The Shield asset reference")]

    [Keywords("Melee", "Combat")]
    [Image(typeof(IconShieldOutline), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionMeleeSetShield : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertySetShield m_To = SetShieldNone.Create;
        [SerializeField] private PropertyGetShield m_Shield = new PropertyGetShield();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_To} = {this.m_Shield}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Shield shield = this.m_Shield.Get(args) as Shield;
            if (shield == null) return DefaultResult;
            
            this.m_To.Set(shield, args);
            return DefaultResult;
        }
    }
}