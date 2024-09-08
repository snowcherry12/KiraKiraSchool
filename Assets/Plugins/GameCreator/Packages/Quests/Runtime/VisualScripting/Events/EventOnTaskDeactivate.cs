using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Task Deactivate")]
    [Category("Quests/On Task Deactivate")]
    [Description("Executes after a Task from a Journal is deactivated")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconTaskOutline), ColorTheme.Type.TextLight)]

    [Serializable]
    public class EventOnTaskDeactivate : TEventOnTask
    {
        protected override void Subscribe()
        {
            this.Journal.EventTaskDeactivate -= this.OnChange;
            this.Journal.EventTaskDeactivate += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskDeactivate -= this.OnChange;
        }
    }
}