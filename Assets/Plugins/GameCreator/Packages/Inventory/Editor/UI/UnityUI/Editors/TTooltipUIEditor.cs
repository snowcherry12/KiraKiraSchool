using GameCreator.Editor.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    public abstract class TTooltipUIEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty tooltip = this.serializedObject.FindProperty("m_Tooltip");
            SerializedProperty offset = this.serializedObject.FindProperty("m_TooltipOffset");
            SerializedProperty keepInParent = this.serializedObject.FindProperty("m_KeepInParent");
            SerializedProperty input = this.serializedObject.FindProperty("m_InputMouse");
            SerializedProperty fromBag = this.serializedObject.FindProperty("m_FromBag");
            
            root.Add(new PropertyField(tooltip));
            root.Add(new PropertyField(offset));
            root.Add(new PropertyField(keepInParent));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(input));
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(fromBag));
            
            return root;
        }
    }
}