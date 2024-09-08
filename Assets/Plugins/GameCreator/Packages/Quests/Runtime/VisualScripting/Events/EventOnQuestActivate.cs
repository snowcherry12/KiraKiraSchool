using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Quest Activate")]
    [Category("Quests/On Quest Activate")]
    [Description("Executes after a Quest from a Journal is activated")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconQuestOutline), ColorTheme.Type.Blue)]

    [Serializable]
    public class EventOnQuestActivate : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestActivate -= this.OnChange;
            this.Journal.EventQuestActivate += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestActivate -= this.OnChange;
        }
    }
}