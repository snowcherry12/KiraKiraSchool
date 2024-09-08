using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Last Quest Deactivated")]
    [Category("States/Last Quest Deactivated")]
    
    [Image(typeof(IconQuestOutline), ColorTheme.Type.TextLight)]
    [Description("A reference to the last Quest deactivated (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestLastDeDeactivated : PropertyTypeGetQuest
    {
        public override Quest Get(Args args) => Quest.LastQuestDeactivated;
        public override Quest Get(GameObject gameObject) => Quest.LastQuestDeactivated;

        public static PropertyGetQuest Create()
        {
            GetQuestLastDeDeactivated instance = new GetQuestLastDeDeactivated();
            return new PropertyGetQuest(instance);
        }

        public override string String => "Last Deactivated";
    }
}