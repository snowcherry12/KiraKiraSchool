using System.Globalization;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Property))]
    public class 
        ItemPropertyDrawer : PropertyDrawer
    {
        public const string PROP_PROPERTY_ID = "m_PropertyID";
        public const string PROP_VISIBLE = "m_Visible";
        public const string PROP_HIDE = "m_Hide";
        
        public const string PROP_NUMBER = "m_Number";
        public const string PROP_TEXT = "m_Text";
        public const string PROP_ICON = "m_Icon";
        public const string PROP_COLOR = "m_Color";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty propertyID = property.FindPropertyRelative(PROP_PROPERTY_ID);
            SerializedProperty propertyVisible = property.FindPropertyRelative(PROP_VISIBLE);
            SerializedProperty propertyHide = property.FindPropertyRelative(PROP_HIDE);
            
            PropertyField fieldPropertyID = new PropertyField(propertyID);
            PropertyField fieldVisibility = new PropertyField(propertyVisible);
            PropertyField fieldHide = new PropertyField(propertyHide);

            root.Add(fieldPropertyID);
            root.Add(fieldVisibility);
            root.Add(fieldHide);
            
            SerializedProperty icon = property.FindPropertyRelative(PROP_ICON);
            SerializedProperty color = property.FindPropertyRelative(PROP_COLOR);

            PropertyField fieldIcon = new PropertyField(icon);
            PropertyField fieldColor = new PropertyField(color);
            
            root.Add(new SpaceSmall());
            root.Add(fieldIcon);
            root.Add(fieldColor);

            SerializedProperty propertyNumber = property.FindPropertyRelative(PROP_NUMBER);
            SerializedProperty propertyText = property.FindPropertyRelative(PROP_TEXT);
            
            PropertyField fieldNumber = new PropertyField(propertyNumber);
            PropertyField fieldText = new PropertyField(propertyText);
            
            root.Add(new SpaceSmall());
            root.Add(fieldNumber);
            root.Add(fieldText);

            return root;
        }

        public static string GetNumberValue(SerializedProperty property)
        {
            return property
                .FindPropertyRelative(PROP_NUMBER)
                .floatValue
                .ToString(CultureInfo.InvariantCulture);
        }
    }
}