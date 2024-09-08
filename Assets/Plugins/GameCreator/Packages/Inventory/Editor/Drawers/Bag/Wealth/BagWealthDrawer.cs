using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.Wealth
{
    [CustomPropertyDrawer(typeof(BagWealth))]
    public class BagWealthDrawer : PropertyDrawer
    {
        private const string NAME_WEALTH = "GC-Inventory-Bag-Runtime-Wealth";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Bag bag = property.serializedObject.targetObject as Bag;
                if (bag != null) this.RuntimeContent(bag.Wealth, root);
            }

            return root;
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected virtual void RuntimeContent(IBagWealth wealth, VisualElement root)
        {
            List<IdString> currencies = wealth.List;
            if (currencies.Count == 0) return;
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Wealth"));

            VisualElement content = new VisualElement { name = NAME_WEALTH };
            this.RefreshRuntime(wealth, content);

            wealth.EventChange -= OnChange;
            wealth.EventChange += OnChange;

            root.Add(content);
            
            void OnChange(IdString currencyID, int prevValue, int newValue)
            {
                this.RefreshRuntime(wealth, content);
            }
        }

        private void RefreshRuntime(IBagWealth wealth, VisualElement container)
        {
            container.Clear();
            
            List<IdString> currencies = wealth.List;
            if (currencies.Count == 0) return;
            
            foreach (IdString currencyID in currencies)
            {
                int amount = wealth.Get(currencyID);

                TextField text = new TextField
                {
                    label = currencyID.String,
                    value = amount.ToString()
                };

                text.SetEnabled(false);
                container.Add(text);
            }
        }
    }
}