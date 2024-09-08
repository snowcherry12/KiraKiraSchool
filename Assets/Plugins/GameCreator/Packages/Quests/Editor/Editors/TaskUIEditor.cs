using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(TaskUI))]
    public class QuestUITaskUIEditorEditor : TTaskUIEditor
    {
        protected override string Message =>
            "This component is configured by its 'Quest UI' parent component";
    }
}