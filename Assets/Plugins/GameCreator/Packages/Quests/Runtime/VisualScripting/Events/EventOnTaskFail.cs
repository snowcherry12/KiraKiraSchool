using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Task Fail")]
    [Category("Quests/On Task Fail")]
    [Description("Executes after a Task from a Journal is failed")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Red)]

    [Serializable]
    public class EventOnTaskFail : TEventOnTask
    {
        protected override void Subscribe()
        {
            this.Journal.EventTaskFail -= this.OnChange;
            this.Journal.EventTaskFail += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskFail -= this.OnChange;
        }
    }
}