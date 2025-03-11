using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Shot))]
    public class ShotDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty type = property.FindPropertyRelative("m_Type");
            PropertyElement typeSelector = new PropertyElement(type, property.displayName, false);
            
            root.Add(typeSelector);
            return root;
        }
    }
}
