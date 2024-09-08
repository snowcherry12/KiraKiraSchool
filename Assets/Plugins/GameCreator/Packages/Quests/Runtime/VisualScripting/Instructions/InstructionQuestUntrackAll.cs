using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Quest Untrack All")]
    [Description("Stops tracking all Quests that are being tacked")]

    [Category("Quests/Quest Untrack All")]
    
    [Parameter("Journal", "The Journal component that tracks the Quest")]
    [Image(typeof(IconBookmarkOutline), ColorTheme.Type.TextLight, typeof(OverlayDot))]
    
    [Keywords("Mission", "Follow", "Bookmark", "Track")]
    [Serializable]
    public class InstructionQuestUntrackAll : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Untrack All";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override System.Threading.Tasks.Task Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return DefaultResult;

            journal.UntrackQuests();
            return DefaultResult;
        }
    }
}