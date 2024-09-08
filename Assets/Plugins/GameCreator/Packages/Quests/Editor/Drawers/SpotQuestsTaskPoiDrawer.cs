using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(SpotQuestsTaskPoi))]
    public class SpotQuestsTaskPoiDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty journal = property.FindPropertyRelative("m_Journal");
            SerializedProperty task = property.FindPropertyRelative("m_Task");

            root.Add(new PropertyField(journal));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(task));

            SerializedProperty offset = property.FindPropertyRelative("m_Offset");
            SerializedProperty space = property.FindPropertyRelative("m_Space");
            SerializedProperty layers = property.FindPropertyRelative("m_Layers");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(offset));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(space));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(layers));
            
            SerializedProperty fadeOutDistance = property.FindPropertyRelative("m_FadeOutDistance");
            SerializedProperty fadeInDistance = property.FindPropertyRelative("m_FadeInDistance");
            SerializedProperty fadeInPadding = property.FindPropertyRelative("m_FadeInPadding");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(fadeOutDistance, "Fade-Out Distance"));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(fadeInDistance, "Fade-In Distance"));
            root.Add(new PropertyField(fadeInPadding, "Fade-In Padding"));

            return root;
        }
    }
}