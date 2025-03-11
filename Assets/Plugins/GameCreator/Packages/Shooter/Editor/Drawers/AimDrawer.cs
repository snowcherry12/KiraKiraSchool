using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Aim))]
    public class AimDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty minAccuracy = property.FindPropertyRelative("m_MinAccuracy");
            root.Add(new PropertyField(minAccuracy));
            
            SerializedProperty aim = property.FindPropertyRelative("m_Aim");
            PropertyElement aimSelector = new PropertyElement(aim, property.displayName, false);
            
            root.Add(new SpaceSmaller());
            root.Add(aimSelector);
            return root;
        }
    }
}
