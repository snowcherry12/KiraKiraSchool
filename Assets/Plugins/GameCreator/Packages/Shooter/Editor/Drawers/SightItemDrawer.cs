using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(SightItem))]
    public class SightItemDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(property.FindPropertyRelative("m_Id")));
            root.Add(new PropertyField(property.FindPropertyRelative("m_Sight")));
            
            root.Add(new SpaceSmall());
            SerializedProperty scopeThrough = property.FindPropertyRelative("m_ScopeThrough");
            SerializedProperty scopePosition = property.FindPropertyRelative("m_ScopePosition");
            SerializedProperty scopeRotation = property.FindPropertyRelative("m_ScopeRotation");
            SerializedProperty scopeDistance = property.FindPropertyRelative("m_ScopeDistance");
            
            PropertyField fieldScopeThrough = new PropertyField(scopeThrough);
            PropertyField fieldScopePosition = new PropertyField(scopePosition, "Position");
            PropertyField fieldScopeRotation = new PropertyField(scopeRotation, "Rotation");
            PropertyField fieldScopeDistance = new PropertyField(scopeDistance, "Distance");
            
            root.Add(fieldScopeThrough);
            root.Add(fieldScopePosition);
            root.Add(fieldScopeRotation);
            root.Add(fieldScopeDistance);
            
            fieldScopeThrough.RegisterValueChangeCallback(_ =>
            {
                if (scopeThrough.boolValue)
                {
                    fieldScopePosition.style.display = DisplayStyle.Flex;
                    fieldScopeRotation.style.display = DisplayStyle.Flex;
                    fieldScopeDistance.style.display = DisplayStyle.Flex;
                }
                else
                {
                    fieldScopePosition.style.display = DisplayStyle.None;
                    fieldScopeRotation.style.display = DisplayStyle.None;
                    fieldScopeDistance.style.display = DisplayStyle.None;
                }
            });
            
            if (scopeThrough.boolValue)
            {
                fieldScopePosition.style.display = DisplayStyle.Flex;
                fieldScopeRotation.style.display = DisplayStyle.Flex;
                fieldScopeDistance.style.display = DisplayStyle.Flex;
            }
            else
            {
                fieldScopePosition.style.display = DisplayStyle.None;
                fieldScopeRotation.style.display = DisplayStyle.None;
                fieldScopeDistance.style.display = DisplayStyle.None;
            }

            return root;
        }
    }
}