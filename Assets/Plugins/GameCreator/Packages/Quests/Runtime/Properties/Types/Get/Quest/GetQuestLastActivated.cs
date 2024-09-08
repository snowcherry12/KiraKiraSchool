using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Last Quest Activated")]
    [Category("States/Last Quest Activated")]
    
    [Image(typeof(IconQuestOutline), ColorTheme.Type.Blue)]
    [Description("A reference to the last Quest activated (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestLastActivated : PropertyTypeGetQuest
    {
        public override Quest Get(Args args) => Quest.LastQuestActivated;
        public override Quest Get(GameObject gameObject) => Quest.LastQuestActivated;

        public static PropertyGetQuest Create()
        {
            GetQuestLastActivated instance = new GetQuestLastActivated();
            return new PropertyGetQuest(instance);
        }

        public override string String => "Last Activated";
    }
}