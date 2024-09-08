using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Properties))]
    public class PropertiesDrawer : TBoxDrawer
    {
        private const string PROP_LIST = "m_List";
        
        private const string USS_PATH = EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/PropertiesOverrides";
        private const string NAME_OVERRIDE = "GC-Inventory-Properties-Overrides-Element";

        protected override string Name(SerializedProperty property) => "Properties";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty inheritFromParent = property.FindPropertyRelative("m_InheritFromParent");
            
            PropertyField fieldInheritFromParent = new PropertyField(inheritFromParent);
            PropertiesTool fieldProperties = new PropertiesTool(property, PROP_LIST);

            VisualElement contentOverrides = new VisualElement();
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) contentOverrides.styleSheets.Add(sheet);
            
            container.Add(fieldInheritFromParent);
            container.Add(contentOverrides);
            container.Add(fieldProperties);

            this.RefreshContentOverrides(contentOverrides, property);
            fieldInheritFromParent.RegisterValueChangeCallback(changeEvent =>
            {
                contentOverrides.Clear();
                if (!changeEvent.changedProperty.boolValue) return;
                this.RefreshContentOverrides(contentOverrides, property);
            });
        }

        private void RefreshContentOverrides(VisualElement content, SerializedProperty property)
        {
            property.serializedObject.Update();
            
            SerializedProperty overrides = property.FindPropertyRelative("m_Overrides");
            SerializedProperty keys = overrides.FindPropertyRelative(PropertiesOverrides.NAME_KEYS);
            SerializedProperty values = overrides.FindPropertyRelative(PropertiesOverrides.NAME_VALUES);

            for (int i = 0; i < keys.arraySize; ++i)
            {
                SerializedProperty key = keys.GetArrayElementAtIndex(i);
                SerializedProperty value = values.GetArrayElementAtIndex(i);
                
                string name = key.FindPropertyRelative(IdStringDrawer.NAME_STRING).stringValue;
                
                Toggle fieldToggle = new Toggle
                {
                    bindingPath = value.FindPropertyRelative("m_Override").propertyPath
                };

                PropertyField fieldNumber = new PropertyField(
                    value.FindPropertyRelative("m_Number"), 
                    name
                );

                fieldToggle.Bind(property.serializedObject);
                fieldNumber.Bind(property.serializedObject);

                fieldNumber.SetEnabled(fieldToggle.value);
                fieldToggle.RegisterValueChangedCallback(changeEvent =>
                {
                    fieldNumber.SetEnabled(changeEvent.newValue);
                });

                VisualElement element = new VisualElement { name = NAME_OVERRIDE };
                
                element.Add(fieldToggle);
                element.Add(fieldNumber);
                content.Add(element);
            }
        }
    }
}