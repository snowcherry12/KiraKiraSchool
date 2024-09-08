using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Expression")]
    
    [Serializable]
    public class Expression : TPolymorphicItem<Expression>, IExpression
    {
        public const string DEFAULT_NAME = "my-expression";
        public const string NAME_ID = nameof(m_Id);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private IdString m_Id = new IdString(DEFAULT_NAME);
        [SerializeField] private PropertyGetSprite m_Sprite = GetSpriteInstance.Create();
        
        [SerializeField] private RunInstructionsList m_InstructionsOnStart = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_InstructionsOnEnd = new RunInstructionsList();
        
        [SerializeField] private SpeechSkin m_OverrideSpeechSkin;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString Id => this.m_Id;
        
        public SpeechSkin OverrideSpeechSkin => this.m_OverrideSpeechSkin;

        // PUBLIC GETTERS: ------------------------------------------------------------------------
        
        public Sprite GetSprite(Args args)
        {
            return this.m_Sprite.Get(args);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public async Task OnStart(Args args)
        {
            RunnerConfig config = new RunnerConfig
            {
                Name = $"On Start Expression {TextUtils.Humanize(this.m_Id.String)}",
                Location = new RunnerLocationLocation(
                    args.Self != null ? args.Self.transform.position : Vector3.zero,
                    args.Self != null ? args.Self.transform.rotation : Quaternion.identity
                )
            };

            await this.m_InstructionsOnStart.Run(args.Clone, config);
        }

        public async Task OnEnd(Args args)
        {
            RunnerConfig config = new RunnerConfig
            {
                Name = $"On End Expression {TextUtils.Humanize(this.m_Id.String)}",
                Location = new RunnerLocationLocation(
                    args.Self != null ? args.Self.transform.position : Vector3.zero,
                    args.Self != null ? args.Self.transform.rotation : Quaternion.identity
                )
            };

            await this.m_InstructionsOnEnd.Run(args.Clone, config);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => this.m_Id.String;
    }
}