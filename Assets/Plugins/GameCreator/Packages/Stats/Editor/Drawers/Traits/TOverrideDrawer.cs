using GameCreator.Editor.Common;
using GameCreator.Editor.Common.ID;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    public abstract class TOverrideDrawer : PropertyDrawer
    {
        public const string NAME_TITLE = "GC-Stats-Traits-Title";
        
        public const string NAME_ELEM_ROOT = "GC-Stats-Traits-Element-Root";
        public const string NAME_ELEM_HEAD = "GC-Stats-Traits-Element-Head";
        public const string NAME_ELEM_BODY = "GC-Stats-Traits-Element-Body";
        
        public const string NAME_HEAD_ICON = "GC-Stats-Traits-Element-Head-Icon";
        public const string NAME_HEAD_TEXT = "GC-Stats-Traits-Element-Head-Text";
        public const string NAME_HEAD_INFO = "GC-Stats-Traits-Element-Head-ExtraInfo";

        private const string IS_EXPANDED = "m_IsExpanded";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract string Label { get; }
        protected abstract IIcon Icon { get; }

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty keys = this.GetKeys(property);
            SerializedProperty values = this.GetValues(property);
            root.Add(new Label
            {
                text = this.Label,
                name = NAME_TITLE
            });

            for (int i = 0; i < keys.arraySize; ++i)
            {
                SerializedProperty key = keys.GetArrayElementAtIndex(i);
                SerializedProperty value = values.GetArrayElementAtIndex(i);
                
                root.Add(this.CreateElement(key, value));
            }
            
            return root;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private VisualElement CreateElement(SerializedProperty key, SerializedProperty value)
        {
            VisualElement root = new VisualElement { name = NAME_ELEM_ROOT };
            VisualElement head = new VisualElement { name = NAME_ELEM_HEAD };
            VisualElement body = new VisualElement { name = NAME_ELEM_BODY };

            string idName = key.FindPropertyRelative(IdStringDrawer.NAME_STRING).stringValue;
            
            Image image = new Image
            {
                image = this.Icon.Texture,
                name = NAME_HEAD_ICON
            };
            
            Label label = new Label
            {
                text = TextUtils.Humanize(idName),
                name = NAME_HEAD_TEXT
            };

            Button headButton = new Button(() =>
            {
                bool isExpanded = !value.FindPropertyRelative(IS_EXPANDED).boolValue;
                value.FindPropertyRelative(IS_EXPANDED).boolValue = isExpanded;
                
                SerializationUtils.ApplyUnregisteredSerialization(value.serializedObject);
                value.serializedObject.Update();

                body.style.display = isExpanded 
                    ? DisplayStyle.Flex 
                    : DisplayStyle.None;
            });

            headButton.Add(image);
            headButton.Add(label);
            head.Add(headButton);

            PropertyField content = new PropertyField(value);
            body.Add(content);
            
            body.style.display = value.FindPropertyRelative(IS_EXPANDED).boolValue 
                ? DisplayStyle.Flex 
                : DisplayStyle.None;

            root.Add(head);
            root.Add(body);
            
            return root;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected abstract SerializedProperty GetKeys(SerializedProperty property);
        protected abstract SerializedProperty GetValues(SerializedProperty property);
    }
}