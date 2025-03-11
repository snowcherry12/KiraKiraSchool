using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Jam))]
    public class JamDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Jam";
    }
}