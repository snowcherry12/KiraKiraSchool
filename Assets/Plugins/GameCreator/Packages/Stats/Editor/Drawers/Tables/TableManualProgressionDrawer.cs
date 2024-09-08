using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(TableManualProgression))]
    public class TableManualProgressionDrawer : PropertyDrawer
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Stats/Editor/StyleSheets/TableManual";

        private const string NAME_ROOT = "GC-Stats-Table-Manual-Root";
        private const string NAME_HEAD = "GC-Stats-Table-Manual-Head";
        private const string NAME_BODY = "GC-Stats-Table-Manual-Body";
        
        private const string CLASS_BODY_ELEMENT = "gc-stats-table-manual-body-element";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement { name = NAME_ROOT };
            VisualElement head = new VisualElement { name = NAME_HEAD };
            VisualElement body = new VisualElement { name = NAME_BODY };

            root.Add(head);
            root.Add(body);
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) root.styleSheets.Add(sheet);
            
            SerializedProperty experience = property.FindPropertyRelative("m_Increments");

            Label headLabel = new Label("Increments");
            IntegerField headSize = new IntegerField
            {
                isDelayed = true,
                value = experience.arraySize
            };
            
            head.Add(headLabel);
            head.Add(headSize);

            CreateBody(body, experience);
            headSize.RegisterValueChangedCallback(changeEvent =>
            {
                experience.arraySize = changeEvent.newValue;
                
                experience.serializedObject.ApplyModifiedProperties();
                experience.serializedObject.Update();
                
                CreateBody(body, experience);
            });

            return root;
        }

        private static void CreateBody(VisualElement body, SerializedProperty experience)
        {
            body.Clear();

            for (int i = 0; i < experience.arraySize; ++i)
            {
                VisualElement content = new VisualElement();
                content.AddToClassList(CLASS_BODY_ELEMENT);
                
                PropertyField fieldElement = new PropertyField(
                    experience.GetArrayElementAtIndex(i), 
                    $"Level {i + 1}"
                );

                fieldElement.Bind(experience.serializedObject);
                
                content.Add(fieldElement);
                body.Add(content);
            }
        }
    }
}