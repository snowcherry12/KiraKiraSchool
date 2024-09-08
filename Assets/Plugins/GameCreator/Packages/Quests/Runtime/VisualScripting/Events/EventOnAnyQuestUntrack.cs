using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("On Any Quest Untrack")]
    [Category("Quests/On Any Quest Untrack")]
    [Description("Executes after a Quest from a Journal stops being tracked")]
    
    [Keywords("Journal", "Mission")]
    [Keywords("Follow")]
    
    [Image(typeof(IconBookmarkOutline), ColorTheme.Type.TextLight)]

    [Serializable]
    public class EventOnAnyQuestUntrack : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestUntrack -= this.OnChange;
            this.Journal.EventQuestUntrack += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestUntrack -= this.OnChange;
        }
    }
}