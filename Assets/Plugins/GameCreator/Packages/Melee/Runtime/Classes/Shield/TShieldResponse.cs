using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public abstract class TShieldResponse
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Reaction m_Reaction;
        [SerializeField] private PropertyGetGameObject m_Effect = GetGameObjectNone.Create();
        [SerializeField] private RunInstructionsList m_InstructionsList = new RunInstructionsList();

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Run(Args args, ShieldOutput shieldOutput, ReactionInput reactionInput)
        {
            if (this.m_Reaction != null)
            {
                Character character = args.Self.Get<Character>();
                this.m_Reaction.Run(character, args, reactionInput);
            }
            
            GameObject effect = this.m_Effect.Get(args);
            if (effect != null)
            {
                PoolManager.Instance.Pick(
                    effect,
                    shieldOutput.Point,
                    SkillEffects.GetRotation(reactionInput.Direction),
                    SkillEffects.POOL_COUNT,
                    SkillEffects.POOL_DURATION
                );
            }
            
            _ = this.m_InstructionsList.Run(args);
        }
    }
}