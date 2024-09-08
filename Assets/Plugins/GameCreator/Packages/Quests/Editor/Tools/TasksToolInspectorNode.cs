using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class TasksToolInspectorNode : VisualElement
    {
        private const string PROPERTY_DATA = TTreeDataItem<Task>.NAME_VALUE;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        private TasksTool ContentTool { get; }
        private SerializedProperty Property { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public TasksToolInspectorNode(TasksTool tasksTool, SerializedProperty property)
        {
            this.AddToClassList(AlignLabel.CLASS_UNITY_INSPECTOR_ELEMENT);
            this.AddToClassList(AlignLabel.CLASS_UNITY_MAIN_CONTAINER);
            
            this.ContentTool = tasksTool;
            this.Property = property.FindPropertyRelative(PROPERTY_DATA);
            
            this.Add(new PropertyField(this.Property));
            this.Bind(this.ContentTool.SerializedObject);
        }
    }
}