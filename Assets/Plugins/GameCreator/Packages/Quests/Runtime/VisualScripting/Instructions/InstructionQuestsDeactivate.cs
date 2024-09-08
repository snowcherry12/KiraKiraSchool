using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Deactivate")]
    [Description("Changes the state of a Quest and its Tasks on a Journal component to Inactive")]

    [Category("Quests/Quest Deactivate")]
    
    [Parameter("Journal", "The Journal component that changes the state of the Quest")]
    [Parameter("Quest", "The Quest asset reference")]
    [Parameter("Wait to Complete", "Whether to wait until the Quest finishes running its Instructions")]

    [Image(typeof(IconQuestOutline), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Keywords("Mission", "Start", "Deactivate", "Inactive", "Disable")]
    [Serializable]
    public class InstructionQuestsDeactivate : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyGetQuest m_Quest = new PropertyGetQuest();
        [SerializeField] private bool m_WaitToComplete;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Deactivate {this.m_Quest}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return;

            Quest quest = this.m_Quest.Get(args);
            if (quest == null) return;

            if (this.m_WaitToComplete) await journal.DeactivateQuest(quest);
            else _ = journal.DeactivateQuest(quest);
        }
    }
}