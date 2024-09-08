using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(DialogueChoiceUI))]
    public class DialogueChoiceUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty text = this.serializedObject.FindProperty("m_Text");
            SerializedProperty index = this.serializedObject.FindProperty("m_Index");
            SerializedProperty button = this.serializedObject.FindProperty("m_Button");
            SerializedProperty actor = this.serializedObject.FindProperty("m_Actor");
            SerializedProperty actorName = this.serializedObject.FindProperty("m_ActorName");
            SerializedProperty actorDesc = this.serializedObject.FindProperty("m_ActorDescription");

            SerializedProperty activeSelected = this.serializedObject.FindProperty("m_ActiveSelected");
            SerializedProperty activeCondition = this.serializedObject.FindProperty("m_ActiveCondition");

            SerializedProperty graphic = this.serializedObject.FindProperty("m_Graphic");
            SerializedProperty graphicNormal = this.serializedObject.FindProperty("m_GraphicNormal");
            SerializedProperty graphicSelected = this.serializedObject.FindProperty("m_GraphicSelected");
            
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            SerializedProperty colorNormal = this.serializedObject.FindProperty("m_ColorNormal");
            SerializedProperty colorVisited = this.serializedObject.FindProperty("m_ColorVisited");

            this.m_Root.Add(new PropertyField(text));
            this.m_Root.Add(new PropertyField(index));
            this.m_Root.Add(new PropertyField(button));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(actor));
            this.m_Root.Add(new PropertyField(actorName));
            this.m_Root.Add(new PropertyField(actorDesc));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(activeSelected));
            this.m_Root.Add(new PropertyField(activeCondition));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(graphic));
            this.m_Root.Add(new PropertyField(graphicNormal));
            this.m_Root.Add(new PropertyField(graphicSelected));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(color));
            this.m_Root.Add(new PropertyField(colorNormal));
            this.m_Root.Add(new PropertyField(colorVisited));

            return this.m_Root;
        }
    }
}