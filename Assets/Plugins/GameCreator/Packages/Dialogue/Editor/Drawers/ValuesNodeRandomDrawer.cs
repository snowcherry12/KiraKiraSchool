using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(ValuesNodeRandom))]
    public class ValuesNodeRandomDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Random";
    }
}