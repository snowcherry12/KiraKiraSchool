using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Activate")]
    [Description("Changes the state of a Quest on a Journal component to Active")]

    [Category("Quests/Quest Activate")]
    
    [Parameter("Journal", "The Journal component that changes the state of the Quest")]
    [Parameter("Quest", "The Quest asset reference")]
    [Parameter("Wait to Complete", "Whether to wait until the Quest finishes running its Instructions")]

    [Image(typeof(IconQuestOutline), ColorTheme.Type.Blue, typeof(OverlayTick))]
    
    [Keywords("Mission", "Start", "Active", "Enable")]
    [Serializable]
    public class InstructionQuestsActivate : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyGetQuest m_Quest = new PropertyGetQuest();
        [SerializeField] private bool m_WaitToComplete;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Activate {this.m_Quest}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return;

            Quest quest = this.m_Quest.Get(args);
            if (quest == null) return;

            if (this.m_WaitToComplete) await journal.ActivateQuest(quest);
            else _ = journal.ActivateQuest(quest);
        }
    }
}