using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Track")]
    [Description("Starts tracking a Quest if it is active")]

    [Category("Quests/Quest Track")]
    
    [Parameter("Journal", "The Journal component that starts tracking the Quest")]
    [Parameter("Quest", "The Quest asset reference")]

    [Image(typeof(IconBookmarkSolid), ColorTheme.Type.Red)]
    
    [Keywords("Mission", "Follow", "Bookmark")]
    [Serializable]
    public class InstructionQuestTrack : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetQuest m_Quest = new PropertyGetQuest();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Track {this.m_Quest}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return DefaultResult;

            Quest quest = this.m_Quest.Get(args);
            if (quest == null) return DefaultResult;

            journal.TrackQuest(quest);
            return DefaultResult;
        }
    }
}