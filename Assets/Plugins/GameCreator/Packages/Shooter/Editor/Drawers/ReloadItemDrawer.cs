using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(ReloadItem))]
    public class ReloadItemDrawer : PropertyDrawer
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