using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Property Text")]
    [Category("Inventory/Item Property Text")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Sets the property Text of the Item")]
    
    [Serializable]
    public class SetStringItemPropertyText : PropertyTypeSetString
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override string Get(Args args) => this.GetText(args);

        public override void Set(string value, Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;
            
            Item item = this.m_Item.Get(args);
            if (item == null) return;

            RuntimeItem runtimeItem = bag.Content.FindRuntimeItem(item);
            
            if (runtimeItem == null) return;

            if (runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop))
            {
                prop.Text = value;
            }
        }

        private string GetText(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return string.Empty;
            
            Item item = this.m_Item.Get(args);
            if (item == null) return string.Empty;

            RuntimeItem runtimeItem = bag.Content.FindRuntimeItem(item);
            if (runtimeItem == null) return string.Empty;
            
            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Text
                : string.Empty;
        }

        public override string String => $"{this.m_Item}[{this.m_PropertyId}] Text";
    }
}