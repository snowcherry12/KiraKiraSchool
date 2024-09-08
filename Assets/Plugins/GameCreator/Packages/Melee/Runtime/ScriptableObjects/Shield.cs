using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [CreateAssetMenu(
        fileName = "Shield (Melee)",
        menuName = "Game Creator/Melee/Shield",
        order = 50
    )]

    [Icon(EditorPaths.PACKAGES + "Melee/Editor/Gizmos/GizmoMeleeShield.png")]

    [Serializable]
    public class Shield : ScriptableObject, IShield
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private int m_Priority = 1;
        
        [SerializeField] private PropertyGetDecimal m_Angle = GetDecimalDecimal.Create(180f);
        [SerializeField] private PropertyGetDecimal m_ParryTime = GetDecimalDecimal.Create(0.25f);

        [SerializeField] private PropertyGetDecimal m_Defense = GetDecimalDecimal.Create(10f);
        [SerializeField] private PropertyGetDecimal m_Cooldown = GetDecimalDecimal.Create(1f);
        [SerializeField] private PropertyGetDecimal m_Recovery = GetDecimalDecimal.Create(1f);
        
        [SerializeField] private StateData m_State = new StateData(StateData.StateType.State);
        [SerializeField] private int m_Layer = Combat.DEFAULT_LAYER_SHIELD;

        [SerializeField] private PropertyGetDecimal m_Speed = GetDecimalConstantOne.Create;
        
        [SerializeField] private float m_TransitionIn = 0.1f;
        [SerializeField] private float m_TransitionOut = 0.25f;

        [SerializeField] private ShieldResponseBlock m_Block = new ShieldResponseBlock();
        [SerializeField] private ShieldResponseParry m_Parry = new ShieldResponseParry();
        [SerializeField] private ShieldResponseBreak m_Break = new ShieldResponseBreak();

        // PROPERTIES: ----------------------------------------------------------------------------

        public int Priority => this.m_Priority;
        public string Name => this.name;

        // PUBLIC GETTERS: ------------------------------------------------------------------------

        public float GetDefense(Args args) => (float) this.m_Defense.Get(args);
        public float GetCooldown(Args args) => (float) this.m_Cooldown.Get(args);
        public float GetRecovery(Args args) => (float) this.m_Recovery.Get(args);

        public float GetAngle(Args args) => (float) this.m_Angle.Get(args);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual ShieldOutput CanDefend(Character character, Args args, ShieldInput input)
        {
            if (!character.Combat.Block.IsBlocking) return ShieldOutput.NO_BLOCK;

            float defenseAngle = this.GetAngle(args);
            float attackAngle = Vector3.Angle(-input.Direction, Vector3.forward);

            if (attackAngle > defenseAngle * 0.5f) return ShieldOutput.NO_BLOCK;

            float parryTime = (float) this.m_ParryTime.Get(args);
            float elapsedTime = character.Combat.Block.RaiseStartTime >= 0
                ? character.Time.Time - character.Combat.Block.RaiseStartTime
                : -1f;
            
            if (elapsedTime >= 0f && elapsedTime <= parryTime)
            {
                return new ShieldOutput(true, input.Point, elapsedTime, BlockType.Parry);   
            }

            character.Combat.CurrentDefense -= input.Power;
            
            return character.Combat.CurrentDefense <= float.Epsilon 
                ? new ShieldOutput(false, input.Point, elapsedTime, BlockType.Break) 
                : new ShieldOutput(true, input.Point, elapsedTime, BlockType.Block);
        }

        public virtual void OnDefend(Args args, ShieldOutput shieldOutput, ReactionInput reaction)
        {
            if (shieldOutput.Type == BlockType.Break)
            {
                Character character = args.Self.Get<Character>();
                if (character != null) character.Combat.Block.LowerGuard();
            }
            
            switch (shieldOutput.Type)
            {
                case BlockType.Block: this.m_Block.Run(args, shieldOutput, reaction); break;
                case BlockType.Parry: this.m_Parry.Run(args, shieldOutput, reaction); break;
                case BlockType.Break: this.m_Break.Run(args, shieldOutput, reaction); break;
                case BlockType.None: throw new ArgumentException();
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void OnRaise(Character character)
        {
            float speed = (float) this.m_Speed.Get(character.gameObject);
            ConfigState config = new ConfigState(
                0f, speed, 1f, 
                this.m_TransitionIn,
                this.m_TransitionOut
            );

            _ = character.States.SetState(
                this.m_State, this.m_Layer, 
                BlendMode.Blend, 
                config
            );
        }

        public virtual void OnLower(Character character)
        {
            character.States.Stop(
                this.m_Layer, 0f, 
                this.m_TransitionOut
            );
        }
    }
}