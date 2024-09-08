using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Last Quest Abandoned")]
    [Category("States/Last Quest Abandoned")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Description("A reference to the last Quest abandoned (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestLastAbandoned : PropertyTypeGetQuest
    {
        public override Quest Get(Args args) => Quest.LastQuestAbandoned;
        public override Quest Get(GameObject gameObject) => Quest.LastQuestAbandoned;

        public static PropertyGetQuest Create()
        {
            GetQuestLastAbandoned instance = new GetQuestLastAbandoned();
            return new PropertyGetQuest(instance);
        }

        public override string String => "Last Abandoned";
    }
}