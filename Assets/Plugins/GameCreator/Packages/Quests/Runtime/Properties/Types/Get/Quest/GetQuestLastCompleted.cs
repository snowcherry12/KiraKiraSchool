using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Last Quest Completed")]
    [Category("States/Last Quest Completed")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Green)]
    [Description("A reference to the last Quest completed (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestLastCompleted : PropertyTypeGetQuest
    {
        public override Quest Get(Args args) => Quest.LastQuestCompleted;
        public override Quest Get(GameObject gameObject) => Quest.LastQuestCompleted;

        public static PropertyGetQuest Create()
        {
            GetQuestLastCompleted instance = new GetQuestLastCompleted();
            return new PropertyGetQuest(instance);
        }

        public override string String => "Last Completed";
    }
}