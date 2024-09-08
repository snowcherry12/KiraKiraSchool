using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class Task
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TaskType m_Completion = TaskType.SubtasksInSequence;
        [SerializeField] private bool m_IsHidden;

        [SerializeField] private PropertyGetString m_Name = GetStringString.Create;
        [SerializeField] private PropertyGetString m_Description = GetStringTextArea.Create();

        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;
        [SerializeField] private PropertyGetSprite m_Sprite = GetSpriteNone.Create;

        [SerializeField] private ProgressType m_UseCounter = ProgressType.None;
        [SerializeField] private PropertyGetDecimal m_CountTo = new PropertyGetDecimal(3);

        [SerializeField] private PropertyGetDecimal m_ValueFrom = GetDecimalGlobalName.Create;
        [SerializeField] private RunEvent m_CheckWhen = new RunEvent(new EventOnLateUpdate());

        [SerializeField] private RunInstructionsList m_OnActivate = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnDeactivate = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnComplete = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnAbandon = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnFail = new RunInstructionsList();

        // PROPERTIES: ----------------------------------------------------------------------------

        public TaskType Completion => this.m_Completion;
        
        public bool IsHidden => this.m_IsHidden;
        public ProgressType UseCounter => this.m_UseCounter;

        public string MaximumValueString => this.m_CountTo.ToString();
        public PropertyGetDecimal ValueFrom => this.m_ValueFrom;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public string GetName(Args args) => this.m_Name.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);

        public Color GetColor(Args args) => this.m_Color.Get(args);
        public Sprite GetSprite(Args args) => this.m_Sprite.Get(args);

        public double GetCountTo(Args args) => this.m_UseCounter switch
        {
            ProgressType.None => 0,
            ProgressType.Value => this.m_CountTo.Get(args),
            ProgressType.Property => this.m_CountTo.Get(args),
            _ => throw new ArgumentOutOfRangeException()
        };

        public async System.Threading.Tasks.Task RunOnActivate(Args args)
        {
            await this.m_OnActivate.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Activate Task",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnDeactivate(Args args)
        {
            await this.m_OnDeactivate.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Deactivate Task",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnComplete(Args args)
        {
            await this.m_OnComplete.Run(
                args.Clone,
                new RunnerConfig
                {
                    Name = "On Complete Task",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnAbandon(Args args)
        {
            await this.m_OnAbandon.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Abandon Task",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnFail(Args args)
        {
            await this.m_OnFail.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Fail Task",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }

        public Trigger CreateCheckWhen(InstructionList instructions)
        {
            return this.m_CheckWhen.Start("Task Detection", instructions);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            string name = this.m_Name.ToString();
            return !string.IsNullOrEmpty(name) ? name : "(no name)";
        }
    }
}