using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ClipMeleeMotionWarping))]
    public class ClipMeleeMotionWarpingDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty conditions = property.FindPropertyRelative("m_Conditions");
            SerializedProperty easing = property.FindPropertyRelative("m_Easing");
            
            SerializedProperty self = property.FindPropertyRelative("m_Self");
            SerializedProperty target = property.FindPropertyRelative("m_Target");

            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(conditions));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(easing));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(self));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(target));

            return root;
        }
    }
}