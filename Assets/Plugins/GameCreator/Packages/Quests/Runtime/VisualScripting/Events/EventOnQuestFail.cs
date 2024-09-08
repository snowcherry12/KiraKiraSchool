using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Quest Fail")]
    [Category("Quests/On Quest Fail")]
    [Description("Executes after a Quest from a Journal is failed")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Red)]

    [Serializable]
    public class EventOnQuestFail : TEventOnQuest
    {
        protected override void Subscribe()
        {
            this.Journal.EventQuestFail -= this.OnChange;
            this.Journal.EventQuestFail += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventQuestFail -= this.OnChange;
        }
    }
}