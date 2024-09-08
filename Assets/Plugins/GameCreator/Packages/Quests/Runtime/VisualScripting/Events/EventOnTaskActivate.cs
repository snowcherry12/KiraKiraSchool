using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Task Activate")]
    [Category("Quests/On Task Activate")]
    [Description("Executes after a Task from a Journal is activated")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconTaskOutline), ColorTheme.Type.Blue)]

    [Serializable]
    public class EventOnTaskActivate : TEventOnTask
    {
        protected override void Subscribe()
        {
            this.Journal.EventTaskActivate -= this.OnChange;
            this.Journal.EventTaskActivate += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskActivate -= this.OnChange;
        }
    }
}