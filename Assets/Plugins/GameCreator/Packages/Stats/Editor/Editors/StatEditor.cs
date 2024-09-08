using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomEditor(typeof(Stat))]
    public class StatEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty id = this.serializedObject.FindProperty("m_ID");
            SerializedProperty data = this.serializedObject.FindProperty("m_Data");
            SerializedProperty info = this.serializedObject.FindProperty("m_Info");

            PropertyField fieldID = new PropertyField(id);
            PropertyField fieldData = new PropertyField(data);
            PropertyField fieldInfo = new PropertyField(info);

            root.Add(fieldID);
            root.Add(fieldData);
            root.Add(fieldInfo);

            return root;
        }
    }
}