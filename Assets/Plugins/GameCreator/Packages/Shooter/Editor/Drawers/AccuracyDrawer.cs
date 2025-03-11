using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Accuracy))]
    public class AccuracyDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Accuracy";
    }
}