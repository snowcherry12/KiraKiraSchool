using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Task Abandon")]
    [Category("Quests/On Task Abandon")]
    [Description("Executes after a Task from a Journal is abandoned")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Yellow)]

    [Serializable]
    public class EventOnTaskAbandon : TEventOnTask
    {
        protected override void Subscribe()
        {
            this.Journal.EventTaskAbandon -= this.OnChange;
            this.Journal.EventTaskAbandon += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskAbandon -= this.OnChange;
        }
    }
}