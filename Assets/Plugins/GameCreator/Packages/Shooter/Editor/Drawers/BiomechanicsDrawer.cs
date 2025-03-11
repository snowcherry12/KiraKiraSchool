using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Biomechanics))]
    public class BiomechanicsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty align = property.FindPropertyRelative("m_Value");
            PropertyElement alignSelector = new PropertyElement(align, property.displayName, false);
            
            root.Add(alignSelector);
            return root;
        }
    }
}
