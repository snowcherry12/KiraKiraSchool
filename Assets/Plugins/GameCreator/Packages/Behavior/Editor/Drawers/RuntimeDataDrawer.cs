using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(RuntimeData))]
    public class RuntimeDataDrawer : PropertyDrawer
    {
        private const string TITLE = "Parameters";
        
        private const string NAME_ROOT = "GC-Behavior-Processor-Data-Root";
        private const string NAME_HEAD = "GC-Behavior-Processor-Data-Head";
        private const string NAME_BODY = "GC-Behavior-Processor-Data-Body";

        private static readonly IIcon ICON_1 = new IconChevronDown(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_2 = new IconChevronRight(ColorTheme.Type.TextLight);
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement { name = NAME_ROOT };
            VisualElement head = new VisualElement { name = NAME_HEAD };
            VisualElement body = new VisualElement { name = NAME_BODY };
            
            root.Add(head);
            root.Add(body);

            Image headIcon = new Image { pickingMode = PickingMode.Ignore };
            Label headLabel = new Label(TITLE) { pickingMode = PickingMode.Ignore };
            
            head.Add(headIcon);
            head.Add(headLabel);
            
            RefreshHeader(property, headIcon, body);

            bool isPlaymode = EditorApplication.isPlayingOrWillChangePlaymode;
            bool isPrefab = PrefabUtility.IsPartOfPrefabAsset(property.serializedObject.targetObject);

            switch (isPlaymode && !isPrefab)
            {
                case true: RepaintBodyRuntime(property, body); break;
                case false: RepaintBodyEditor(property, body); break;
            }
            
            head.RegisterCallback<ClickEvent>(_ => Toggle(property, headIcon, body));
            
            root.style.display = body.childCount != 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            return root;
        }

        private static void RepaintBodyRuntime(SerializedProperty property, VisualElement body)
        {
            if (body == null) return;
            property.serializedObject.Update();
            
            Processor processor = property.serializedObject.targetObject as Processor;
            if (processor == null) return;
            
            SerializedProperty propertyOverrides = property.FindPropertyRelative("m_Overrides");
            
            for (int i = 0; i < propertyOverrides.arraySize; ++i)
            {
                string parameterName = propertyOverrides
                    .GetArrayElementAtIndex(i)
                    .FindPropertyRelative(ParameterDrawer.PROP_NAME).stringValue;

                object value = processor.RuntimeData.GetParameter(parameterName);
                switch (value)
                {
                    case null: continue;
                    
                    case Object unityObject:
                        ObjectField objectField = new ObjectField(parameterName)
                        {
                            value = unityObject,
                            objectType = value.GetType()
                        };
                    
                        objectField.SetEnabled(false);
                        objectField.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
                    
                        body.Add(objectField);
                        break;
                    
                    
                    default:
                        TextField textField = new TextField(parameterName)
                        {
                            value = value.ToString()
                        };
                
                        textField.SetEnabled(false);
                        textField.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
                    
                        body.Add(textField);
                        break;
                }
            }
        }

        private static void RepaintBodyEditor(SerializedProperty property, VisualElement body)
        {
            if (body == null) return;
            property.serializedObject.Update();
            
            SerializedProperty propertyOverrides = property.FindPropertyRelative("m_Overrides");
            for (int i = 0; i < propertyOverrides.arraySize; ++i)
            {
                SerializedProperty parameter = propertyOverrides.GetArrayElementAtIndex(i);
                PropertyField fieldParameter = new PropertyField(parameter);
                
                body.Add(fieldParameter);
            }
        }

        // PRIVATE STATIC METHODS: ----------------------------------------------------------------
        
        private static void Toggle(SerializedProperty property, Image headIcon, VisualElement body)
        {
            property.serializedObject.Update();
            property.isExpanded = !property.isExpanded;
            
            RefreshHeader(property, headIcon, body);
        }
        
        private static void RefreshHeader(SerializedProperty property, Image headIcon, VisualElement body)
        {
            headIcon.image = property.isExpanded
                ? ICON_1.Texture
                : ICON_2.Texture;
            
            body.style.display = property.isExpanded
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}