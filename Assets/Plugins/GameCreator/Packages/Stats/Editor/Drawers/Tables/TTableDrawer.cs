using GameCreator.Editor.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(TTable), true)]
    public class TTableDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            SerializationUtils.CreateChildProperties(
                root,
                property,
                SerializationUtils.ChildrenMode.ShowLabelsInChildren, 
                false
            );

            return root;
        }
    }
}