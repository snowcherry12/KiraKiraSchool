using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(SelectedQuestUI))]
    public class SelectedQuestUIEditor : TQuestUIEditor
    {
        protected override string Message => 
            "This component is automatically configured by the current selected Quest";

        protected override void CreateAdditionalProperties(VisualElement root)
        {
            base.CreateAdditionalProperties(root);

            SerializedProperty activeIfSelected = this.serializedObject.FindProperty("m_ActiveIfSelected");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(activeIfSelected));
        }
    }
}