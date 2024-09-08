using System;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(NodeStateMachineElbow))]
    public class NodeStateMachineElbowDrawer : PropertyDrawer
    {
        public const string PROP_DIRECTION = "m_Direction";

        private const string PROP_PORT_POSITION = "m_Position";
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty direction = property.FindPropertyRelative(PROP_DIRECTION);
            PropertyField fieldDirection = new PropertyField(direction);

            root.Add(fieldDirection);
            
            fieldDirection.RegisterValueChangeCallback(changeEvent =>
            {
                property.serializedObject.Update();
                SerializedProperty propertyPorts = property.FindPropertyRelative(TNodeTool.PROP_PORTS);

                int value = changeEvent.changedProperty.enumValueIndex;

                SerializedProperty inputPorts = propertyPorts.FindPropertyRelative(TNodeTool.PROP_INPUTS);
                SerializedProperty outputPorts = propertyPorts.FindPropertyRelative(TNodeTool.PROP_OUTPUTS);

                int valueInputPort = EnumIndexInput(value);
                int valueOutputPort = EnumIndexOutput(value);

                for (int i = 0; i < inputPorts.arraySize; ++i)
                {
                    SerializedProperty port = inputPorts.GetArrayElementAtIndex(i);
                    port.FindPropertyRelative(PROP_PORT_POSITION).enumValueIndex = valueInputPort;
                }
                
                for (int i = 0; i < outputPorts.arraySize; ++i)
                {
                    SerializedProperty port = outputPorts.GetArrayElementAtIndex(i);
                    port.FindPropertyRelative(PROP_PORT_POSITION).enumValueIndex = valueOutputPort;
                }

                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                property.serializedObject.Update();
            });

            return root;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        public static int EnumIndexInput(int index)
        {
            return index switch
            {
                0 => (int) PortPosition.Bottom,
                1 => (int) PortPosition.Left,
                2 => (int) PortPosition.Top,
                3 => (int) PortPosition.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
        
        public static int EnumIndexOutput(int index)
        {
            return index switch
            {
                0 => (int) PortPosition.Top,
                1 => (int) PortPosition.Right,
                2 => (int) PortPosition.Bottom,
                3 => (int) PortPosition.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
    }
}