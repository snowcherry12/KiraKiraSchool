using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomPropertyDrawer(typeof(SensorHear))]
    public class SensorHearDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement content = new VisualElement();

            SerializedProperty useObstruction = property.FindPropertyRelative("m_UseObstruction");
            SerializedProperty minIntensity = property.FindPropertyRelative("m_MinIntensity");
            SerializedProperty maxIntensity = property.FindPropertyRelative("m_MaxIntensity");
            SerializedProperty decayTime = property.FindPropertyRelative("m_DecayTime");
            
            content.Add(new PropertyField(useObstruction));
            content.Add(new SpaceSmaller());
            content.Add(new PropertyField(minIntensity));
            content.Add(new SpaceSmaller());
            content.Add(new PropertyField(maxIntensity));
            content.Add(new SpaceSmaller());
            content.Add(new PropertyField(decayTime));
            
            return content;
        }
    }
}