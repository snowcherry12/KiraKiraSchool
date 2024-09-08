using GameCreator.Editor.Cameras;
using GameCreator.Runtime.Dialogue;
using UnityEditor;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(ShotSystemSwitcher))]
    public class ShotTypeSwitcherDrawer : TShotSystemDrawer
    {
        protected override string Name(SerializedProperty property) => "Switcher";
    }
}