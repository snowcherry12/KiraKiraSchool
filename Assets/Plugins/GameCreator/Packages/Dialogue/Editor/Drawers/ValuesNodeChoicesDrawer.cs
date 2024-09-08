using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(ValuesNodeChoices))]
    public class ValuesNodeChoicesDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Choices";
    }
}