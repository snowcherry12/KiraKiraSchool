using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Task Complete")]
    [Category("Quests/On Task Complete")]
    [Description("Executes after a Task from a Journal is completed")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Green)]

    [Serializable]
    public class EventOnTaskComplete : TEventOnTask
    {
        protected override void Subscribe()
        {
            this.Journal.EventTaskComplete -= this.OnChange;
            this.Journal.EventTaskComplete += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskComplete -= this.OnChange;
        }
    }
}