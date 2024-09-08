using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Coins))]
    public class CoinsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            CoinsTool coinsTool = new CoinsTool(property, "m_List");
            root.Add(coinsTool);
            
            return root;
        }
    }
}