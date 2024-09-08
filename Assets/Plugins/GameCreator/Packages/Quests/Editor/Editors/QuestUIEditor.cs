using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(QuestUI))]
    public class QuestUIEditor : TQuestUIEditor
    {
        protected override string Message =>
            "This component is configured by its 'Quest List UI' parent component";
    }
}