using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(NodeAnimation))]
    public class NodeAnimationDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Configure";
    }
}