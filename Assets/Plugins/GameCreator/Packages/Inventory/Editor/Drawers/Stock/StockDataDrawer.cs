using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(StockData))]
    public class StockDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty item = property.FindPropertyRelative("m_Item");
            SerializedProperty amount = property.FindPropertyRelative("m_Amount");

            PropertyField fieldItem = new PropertyField(item);
            PropertyField fieldAmount = new PropertyField(amount);
            
            root.Add(fieldItem);
            root.Add(fieldAmount);
            
            return root;
        }
    }
}