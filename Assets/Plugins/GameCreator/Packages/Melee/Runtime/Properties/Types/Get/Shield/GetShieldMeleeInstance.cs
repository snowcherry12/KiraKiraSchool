using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Melee Shield")]
    [Category("Melee Shield")]
    
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Red)]
    [Description("A reference to a Melee Shield asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetShieldMeleeInstance : PropertyTypeGetShield
    {
        [SerializeField] protected Shield m_Shield;

        public override IShield Get(Args args) => this.m_Shield;
        public override IShield Get(GameObject gameObject) => this.m_Shield;

        public static PropertyGetShield Create(Shield shield = null)
        {
            GetShieldMeleeInstance instance = new GetShieldMeleeInstance
            {
                m_Shield = shield
            };
            
            return new PropertyGetShield(instance);
        }

        public override string String => this.m_Shield != null
            ? this.m_Shield.name
            : "(none)";
    }
}