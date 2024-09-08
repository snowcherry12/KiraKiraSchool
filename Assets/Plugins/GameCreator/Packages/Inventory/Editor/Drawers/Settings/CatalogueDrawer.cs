using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Catalogue))]
    public class CatalogueDrawer : TTitleDrawer
    {
        protected override string Title => "Items";

        protected override void CreateContent(VisualElement body, SerializedProperty property)
        {
            Button buttonRefresh = new Button(InventoryPostProcessor.RefreshItems)
            {
                text = "Refresh",
                style = { height = 25 }
            };

            body.Add(new SpaceSmall());
            body.Add(buttonRefresh);
            body.Add(new SpaceSmall());
            
            SerializedProperty items = property.FindPropertyRelative("m_Items");

            int itemsCount = items.arraySize;
            for (int i = 0; i < itemsCount; ++i)
            {
                SerializedProperty item = items.GetArrayElementAtIndex(i);
                PropertyField itemField = new PropertyField(item, string.Empty);

                itemField.SetEnabled(false);
                body.Add(itemField);
                body.Add(new SpaceSmaller());
            }
        }
    }
}