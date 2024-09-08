using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Title("On Task Value Change")]
    [Category("Quests/On Task Value Change")]
    [Description("Executes after a specific Active Task from a Journal changes its value")]
    
    [Keywords("Journal", "Mission")]
    [Image(typeof(IconTaskSolid), ColorTheme.Type.Blue, typeof(OverlayDot))]

    [Serializable]
    public class EventOnTaskValueChange : TEventOnTask
    {
        protected override void Subscribe()
        {
            this.Journal.EventTaskValueChange -= this.OnChange;
            this.Journal.EventTaskValueChange += this.OnChange;
        }

        protected override void Unsubscribe()
        {
            if (this.Journal == null) return;
            this.Journal.EventTaskValueChange -= this.OnChange;
        }
    }
}