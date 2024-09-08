using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(TBag), true)]
    public class TBagDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            TBagElement bagElement = new TBagElement(property);
            root.Add(bagElement);

            return root;
        }
    }
}