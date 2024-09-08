using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(CoinSelect))]
    public class CoinSelectDrawer : PropertyDrawer
    {
        private const string NAME_FIELD_COIN = "Coin";
        
        private const string PROP_CURRENCY = "m_Currency";
        private const string PROP_COIN_INDEX = "m_CoinIndex";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty currency = property.FindPropertyRelative(PROP_CURRENCY);

            PropertyField fieldCurrency = new PropertyField(currency);
            VisualElement contentCoinIndex = new VisualElement();
            
            root.Add(fieldCurrency);
            root.Add(contentCoinIndex);
            
            this.RefreshCoinIndex(contentCoinIndex, property);
            fieldCurrency.RegisterValueChangeCallback(_ =>
            {
                this.RefreshCoinIndex(contentCoinIndex, property);
            });

            return root;
        }

        private void RefreshCoinIndex(VisualElement content, SerializedProperty property)
        {
            content.Clear();

            SerializedProperty propertyCurrency = property.FindPropertyRelative(PROP_CURRENCY);
            SerializedProperty propertyCoinIndex = property.FindPropertyRelative(PROP_COIN_INDEX);
            
            content.SetEnabled(propertyCurrency.objectReferenceValue != null);

            if (propertyCurrency.objectReferenceValue == null)
            {
                content.Add(new DropdownField(NAME_FIELD_COIN));
                return;
            }
            
            Currency currency = (Currency) propertyCurrency.objectReferenceValue;

            int defaultIndex = propertyCoinIndex.intValue;
            if (defaultIndex >= currency.Coins.Length)
            {
                defaultIndex = 0;
                propertyCoinIndex.intValue = 0;
                
                propertyCoinIndex.serializedObject.ApplyModifiedProperties();
                propertyCoinIndex.serializedObject.Update();
            }

            List<int> indexes = new List<int>();
            List<string> names = new List<string>();

            for (int i = 0; i < currency.Coins.Length; ++i)
            {
                indexes.Add(i);
                names.Add(currency.Coins[i].Name);
            }

            PopupField<int> fieldCoinIndex = new PopupField<int>(
                NAME_FIELD_COIN,
                indexes,
                defaultIndex,
                i => names[i],
                i => $"{i}: {names[i]}"
            );
            
            fieldCoinIndex.RegisterValueChangedCallback(changeEvent =>
            {
                propertyCoinIndex.intValue = changeEvent.newValue;
                propertyCoinIndex.serializedObject.ApplyModifiedProperties();
                propertyCoinIndex.serializedObject.Update();
            });
            
            content.Add(fieldCoinIndex);
            
            fieldCoinIndex.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
            AlignLabel.On(fieldCoinIndex);
        }
    }
}