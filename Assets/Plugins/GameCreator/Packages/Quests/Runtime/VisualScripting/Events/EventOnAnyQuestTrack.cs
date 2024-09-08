using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("On Any Quest Track")]
    [Category("Quests/On Any Quest Track")]
    [Description("Executes after a Quest from a Journal starts being tracked")]
    
    [Keywords("Journal", "Mission")]
    [Keywords("Follow")]
    
    [Image(typeof(IconBookmarkSolid), ColorTheme.Type.Red)]

    [Serializable]
    public class EventOnAnyQuestTrack : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestTrack -= this.OnChange;
            this.Journal.EventQuestTrack += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestTrack -= this.OnChange;
        }
    }
}