using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    public class CombosToolInspectorNode : VisualElement
    {
        private const string PROPERTY_DATA = TTreeDataItem<ComboItem>.NAME_VALUE;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        private CombosTool ContentTool { get; }
        private SerializedProperty Property { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public CombosToolInspectorNode(CombosTool combosTool, SerializedProperty property, bool isRoot)
        {
            this.AddToClassList(AlignLabel.CLASS_UNITY_INSPECTOR_ELEMENT);
            this.AddToClassList(AlignLabel.CLASS_UNITY_MAIN_CONTAINER);
            
            this.ContentTool = combosTool;
            this.Property = property.FindPropertyRelative(PROPERTY_DATA);
            
            VisualElement inspector = ComboItemDrawer.Paint(this.Property, !isRoot);
            this.Add(inspector);
            this.Bind(this.ContentTool.SerializedObject);
        }
    }
}