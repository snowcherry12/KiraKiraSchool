using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(InteractionQuestUI), true)]
    public class InteractionsQuestUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Interactive Elements";
    }
}