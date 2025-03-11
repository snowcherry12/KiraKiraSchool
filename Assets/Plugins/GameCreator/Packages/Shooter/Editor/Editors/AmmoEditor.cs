using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(Ammo))]
    public class AmmoEditor : UnityEditor.Editor
    {
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            StyleSheet[] styleSheets = StyleSheetUtils.Load();
            foreach (StyleSheet styleSheet in styleSheets) root.styleSheets.Add(styleSheet);

            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Id")));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Title")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Description")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Icon")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Color")));
            
            root.Add(new SpaceSmall());

            SerializedProperty infinite = this.serializedObject.FindProperty("m_Infinite");
            SerializedProperty value = this.serializedObject.FindProperty("m_Value");
            
            PropertyField infiniteField = new PropertyField(infinite);
            PropertyField valueField = new PropertyField(value);
            
            root.Add(infiniteField);
            root.Add(valueField);

            valueField.style.display = infinite.boolValue
                ? DisplayStyle.None
                : DisplayStyle.Flex;
            
            infiniteField.RegisterValueChangeCallback(changeEvent =>
            {
                valueField.style.display = changeEvent.changedProperty.boolValue
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            });
            
            return root;
        }
    }
}