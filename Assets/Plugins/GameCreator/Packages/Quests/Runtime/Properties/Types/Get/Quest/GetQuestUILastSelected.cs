using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Last Quest Selected")]
    [Category("UI/Last Quest Selected")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Description("A reference to the last Quest selected (if any)")]

    [Serializable] [HideLabelsInEditor]
    public class GetQuestUILastSelected : PropertyTypeGetQuest
    {
        public override Quest Get(Args args) => QuestUI.UI_LastQuestSelected;
        public override Quest Get(GameObject gameObject) => QuestUI.UI_LastQuestSelected;

        public static PropertyGetQuest Create()
        {
            GetQuestUILastSelected instance = new GetQuestUILastSelected();
            return new PropertyGetQuest(instance);
        }

        public override string String => "Last Selected";
    }
}