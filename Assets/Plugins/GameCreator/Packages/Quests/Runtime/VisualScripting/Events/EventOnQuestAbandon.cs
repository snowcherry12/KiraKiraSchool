using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Quest Abandon")]
    [Category("Quests/On Quest Abandon")]
    [Description("Executes after a Quest from a Journal is abandoned")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]

    [Serializable]
    public class EventOnQuestAbandon : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestAbandon -= this.OnChange;
            this.Journal.EventQuestAbandon += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestAbandon -= this.OnChange;
        }
    }
}