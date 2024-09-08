using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Quest Complete")]
    [Category("Quests/On Quest Complete")]
    [Description("Executes after a Quest from a Journal is completed")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Green)]

    [Serializable]
    public class EventOnQuestComplete : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestComplete -= this.OnChange;
            this.Journal.EventQuestComplete += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestComplete -= this.OnChange;
        }
    }
}