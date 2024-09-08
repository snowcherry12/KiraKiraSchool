using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Socket))]
    public class ItemSocketDrawer : PropertyDrawer
    {
        public const string PROP_BASE = "m_Base";
        public const string PROP_SOCKET_ID = "m_SocketID";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty propertyBase = property.FindPropertyRelative(PROP_BASE);
            SerializedProperty propertySocketID = property.FindPropertyRelative(PROP_SOCKET_ID);

            PropertyField fieldBase = new PropertyField(propertyBase);
            PropertyField fieldSocketID = new PropertyField(propertySocketID);

            root.Add(fieldBase);
            root.Add(fieldSocketID);

            return root;
        }
    }
}