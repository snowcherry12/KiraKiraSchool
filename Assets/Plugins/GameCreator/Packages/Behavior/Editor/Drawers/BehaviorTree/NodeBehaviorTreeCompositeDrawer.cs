using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeBehaviorTreeComposite))]
    public class NodeBehaviorTreeCompositeDrawer : PropertyDrawer
    {
        private const string PROP_STOP = "m_Stop";
        private const string PROP_CONDITIONS = "m_Conditions";
        private const string PROP_COMPOSITE = "m_Composite";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty stop = property.FindPropertyRelative(PROP_STOP);
            SerializedProperty conditions = property.FindPropertyRelative(PROP_CONDITIONS);
            SerializedProperty composite = property.FindPropertyRelative(PROP_COMPOSITE);
            
            PropertyField fieldStop = new PropertyField(stop);
            PropertyField fieldConditions = new PropertyField(conditions);
            
            PropertyElement fieldComposite = new PropertyElement(
                composite,
                composite.displayName,
                false
            );
            
            root.Add(fieldStop);
            root.Add(new SpaceSmaller());
            root.Add(fieldConditions);
            
            root.Add(new SpaceSmall());
            root.Add(new SpaceSmaller());
            root.Add(fieldComposite);

            return root;
        }
    }
}