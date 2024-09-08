using GameCreator.Editor.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomEditor(typeof(StatusEffect))]
    public class StatusEffectEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty data = this.serializedObject.FindProperty("m_Data");
            SerializedProperty info = this.serializedObject.FindProperty("m_Info");
            
            PropertyField fieldData = new PropertyField(data);
            PropertyField fieldInfo = new PropertyField(info);
            
            root.Add(new SpaceSmaller());
            root.Add(fieldData);
            root.Add(new SpaceSmaller());
            root.Add(fieldInfo);

            SerializedProperty onStart = this.serializedObject.FindProperty("m_OnStart");
            SerializedProperty onEnd = this.serializedObject.FindProperty("m_OnEnd");
            SerializedProperty whileActive = this.serializedObject.FindProperty("m_WhileActive");

            PropertyField fieldOnStart = new PropertyField(onStart);
            PropertyField fieldOnEnd = new PropertyField(onEnd);
            PropertyField fieldWhileActive = new PropertyField(whileActive);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Start:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnStart);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On End:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldOnEnd);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("While Active:"));
            root.Add(new SpaceSmaller());
            root.Add(fieldWhileActive);
            
            SerializedProperty id = this.serializedObject.FindProperty("m_ID");
            PropertyField fieldID = new PropertyField(id);
            
            root.Add(new SpaceSmall());
            root.Add(fieldID);

            return root;
        }
    }
}