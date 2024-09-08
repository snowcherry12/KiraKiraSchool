using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Untrack")]
    [Description("Stops tracking a Quest if it is being tacked")]

    [Category("Quests/Quest Untrack")]
    
    [Parameter("Journal", "The Journal component that tracks the Quest")]
    [Parameter("Quest", "The Quest asset reference")]

    [Image(typeof(IconBookmarkOutline), ColorTheme.Type.TextLight)]
    
    [Keywords("Mission", "Follow", "Bookmark", "Track")]
    [Serializable]
    public class InstructionQuestUntrack : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetQuest m_Quest = new PropertyGetQuest();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Untrack {this.m_Quest}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return DefaultResult;

            Quest quest = this.m_Quest.Get(args);
            if (quest == null) return DefaultResult;

            journal.UntrackQuest(quest);
            return DefaultResult;
        }
    }
}