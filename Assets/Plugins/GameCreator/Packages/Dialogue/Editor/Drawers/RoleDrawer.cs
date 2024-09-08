using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(Role))]
    public class RoleDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty actor = property.FindPropertyRelative("m_Actor");
            SerializedProperty target = property.FindPropertyRelative("m_Target");

            string label = actor.objectReferenceValue != null
                ? TextUtils.Humanize(actor.objectReferenceValue.name)
                : "Unknown";

            PropertyElement fieldTarget = new PropertyElement(
                target.FindPropertyRelative(IPropertyDrawer.PROPERTY_NAME),
                label, 
                false
            );
            
            root.Add(fieldTarget);
            return root;
        }
    }
}