using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(InteractionsTaskUI), true)]
    public class InteractionsTaskUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Interactive Elements";
    }
}