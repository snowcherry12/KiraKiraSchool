using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(TActiveUI), true)]
    public class TActiveUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Active Elements";
    }
}