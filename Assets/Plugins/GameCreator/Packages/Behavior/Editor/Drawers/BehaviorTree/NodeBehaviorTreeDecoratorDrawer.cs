using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeBehaviorTreeDecorator))]
    public class NodeBehaviorTreeDecoratorDrawer : PropertyDrawer
    {
        private const string PROP_DECORATOR = "m_Decorator";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty decorator = property.FindPropertyRelative(PROP_DECORATOR);
            PropertyElement fieldDecorator = new PropertyElement(
                decorator,
                decorator.displayName,
                false
            );
            
            root.Add(fieldDecorator);
            return root;
        }
    }
}