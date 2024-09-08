using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Coin))]
    public class CoinDrawer : PropertyDrawer
    {
        internal const string PROP_VALUE = "m_Value";
        internal const string PROP_NAME = "m_Name";
        internal const string PROP_ICON = "m_Icon";
        internal const string PROP_TINT = "m_Tint";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty value = property.FindPropertyRelative(PROP_VALUE);
            SerializedProperty name = property.FindPropertyRelative(PROP_NAME);
            SerializedProperty icon = property.FindPropertyRelative(PROP_ICON);
            SerializedProperty tint = property.FindPropertyRelative(PROP_TINT);

            PropertyField fieldValue = new PropertyField(value);
            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldIcon = new PropertyField(icon);
            PropertyField fieldTint = new PropertyField(tint);

            root.Add(fieldValue);
            root.Add(new SpaceSmall());
            root.Add(fieldName);
            root.Add(new SpaceSmall());
            root.Add(fieldIcon);
            root.Add(new SpaceSmall());
            root.Add(fieldTint);

            fieldValue.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            
            return root;
        }
    }
}