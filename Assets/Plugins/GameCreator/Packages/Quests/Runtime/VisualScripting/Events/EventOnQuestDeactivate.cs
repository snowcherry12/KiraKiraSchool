using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Quest Deactivate")]
    [Category("Quests/On Quest Deactivate")]
    [Description("Executes after a Quest from a Journal is deactivated")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconQuestOutline), ColorTheme.Type.TextLight)]

    [Serializable]
    public class EventOnQuestDeactivate : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestDeactivate -= this.OnChange;
            this.Journal.EventQuestDeactivate += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestDeactivate -= this.OnChange;
        }
    }
}