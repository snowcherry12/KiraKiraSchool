using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Last Quest Failed")]
    [Category("States/Last Quest Failed")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Red)]
    [Description("A reference to the last Quest failed (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestLastFailed : PropertyTypeGetQuest
    {
        public override Quest Get(Args args) => Quest.LastQuestFailed;
        public override Quest Get(GameObject gameObject) => Quest.LastQuestFailed;

        public static PropertyGetQuest Create()
        {
            GetQuestLastFailed instance = new GetQuestLastFailed();
            return new PropertyGetQuest(instance);
        }

        public override string String => "Last Failed";
    }
}