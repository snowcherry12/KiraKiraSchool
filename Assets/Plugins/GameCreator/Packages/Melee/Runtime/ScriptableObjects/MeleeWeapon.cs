using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [CreateAssetMenu(
        fileName = "Weapon (Melee)", 
        menuName = "Game Creator/Melee/Weapon",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Melee/Editor/Gizmos/GizmoMeleeWeapon.png")]
    
    [Serializable]
    public class MeleeWeapon : TWeapon
    {
        private const float TRANSITION = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Shield m_Shield;

        [SerializeField] private StateData m_State = new StateData(StateData.StateType.State);
        [SerializeField] private PropertyGetInteger m_Layer = GetDecimalInteger.Create(5);

        [SerializeField] private ComboSelector m_Combos = new ComboSelector();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override IShield Shield => this.m_Shield;

        public override Texture EditorIcon => new IconMeleeSword(
            ColorTheme.Get(ColorTheme.Type.Yellow)
        ).Texture;

        public ComboTree Combo => this.m_Combos.Get;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override async Task RunOnEquip(Character character, Args args)
        {
            if (this.m_State.IsValid(character))
            {
                ConfigState configuration = new ConfigState(0f, 1f, 1f, TRANSITION, 0f);
            
                _ = character.States.SetState(
                    this.m_State, (int) this.m_Layer.Get(args), 
                    BlendMode.Blend, configuration
                );
            }

            await base.RunOnEquip(character, args);
        }

        public override async Task RunOnUnequip(Character character, Args args)
        {
            if (this.m_State.IsValid(character))
            {
                int layer = (int) this.m_Layer.Get(args);
                character.States.Stop(layer, 0f, TRANSITION);
            }
            
            await base.RunOnUnequip(character, args);
        }

        public override TMunitionValue CreateMunition()
        {
            return new MeleeMunition();
        }
    }
}