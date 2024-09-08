using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public abstract class TNodeTypeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();

            root.Add(head);
            root.Add(body);

            SerializedProperty options = property.FindPropertyRelative("m_Options");
            PropertyField fieldOptions = new PropertyField(options);

            head.Add(fieldOptions);
            this.SetupBody(property, body);

            fieldOptions.RegisterValueChangeCallback(changeEvent =>
            {
                int index = changeEvent.changedProperty.enumValueIndex;
                body.style.display = index == (int) NodeTypeData.FromNode
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            body.style.display = options.enumValueIndex == (int) NodeTypeData.FromNode
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            return root;
        }
        
        protected abstract void SetupBody(SerializedProperty property, VisualElement body);
    }
}