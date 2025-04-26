using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(DriverNavmeshArea))]
    public class DriverNavmeshAreaDrawer : PropertyDrawer
    {
        private static string[] AreaNames;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            AreaNames = NavMesh.GetAreaNames();
            List<int> options = new List<int>(AreaNames.Length);
            
            for (int i = 0; i < AreaNames.Length; ++i)
            {
                if (string.IsNullOrEmpty(AreaNames[i])) continue;
                options.Add(i);
            }
            
            SerializedProperty area = property.FindPropertyRelative("m_Area");

            PopupField<int> popupField = new PopupField<int>(
                property.displayName,
                options, 
                area.intValue,
                IndexToAreaName,
                IndexToAreaName
            );

            popupField.RegisterValueChangedCallback(changeEvent =>
            {
                area.intValue = changeEvent.newValue;
                area.serializedObject.ApplyModifiedProperties();
                area.serializedObject.Update();
            });

            popupField.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
            AlignLabel.On(popupField);
            
            return popupField;
        }

        private static string IndexToAreaName(int index)
        {
            AreaNames = NavMesh.GetAreaNames();
            return index < AreaNames.Length ? AreaNames[index] : string.Empty;
        }
    }
}