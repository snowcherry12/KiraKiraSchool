using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Shell))]
    public class ShellDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Shell";
    }
}