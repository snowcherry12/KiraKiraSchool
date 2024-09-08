using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Stock))]
    public class StockDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            StockTool toolStock = new StockTool(property, "m_Stock");
            WealthTool toolWealth = new WealthTool(property, "m_Wealth");
            
            root.Add(toolStock);
            root.Add(new SpaceSmall());
            root.Add(toolWealth);

            return root;
        }
    }
}