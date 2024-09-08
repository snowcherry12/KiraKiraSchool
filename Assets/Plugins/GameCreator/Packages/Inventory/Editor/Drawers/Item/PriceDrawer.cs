using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Price))]
    public class PriceDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Price";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty currency = property.FindPropertyRelative("m_Currency");
            SerializedProperty value = property.FindPropertyRelative("m_Value");

            if (currency.objectReferenceValue == null)
            {
                currency.objectReferenceValue = CurrencyEditor.DEFAULT_INSTANCE;
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                property.serializedObject.Update();
            }

            PropertyField fieldCurrency = new PropertyField(currency);
            PropertyField fieldPrice = new PropertyField(value);

            container.Add(fieldCurrency);
            container.Add(fieldPrice);
            
            SerializedProperty canBuy = property.FindPropertyRelative("m_CanBuyFromMerchant");
            SerializedProperty canSell = property.FindPropertyRelative("m_CanSellToMerchant");
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(canBuy));
            container.Add(new PropertyField(canSell));
        }
    }
}