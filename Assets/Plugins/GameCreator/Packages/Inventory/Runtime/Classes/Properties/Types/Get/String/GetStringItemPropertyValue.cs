using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Property Value")]
    [Category("Inventory/Item Property Value")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Returns the property Value of the Item")]
    
    [Serializable]
    public class GetStringItemPropertyValue : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override string Get(Args args) => this.GetValue(args);

        private string GetValue(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return string.Empty;
            
            Item item = this.m_Item.Get(args);
            if (item == null) return string.Empty;

            RuntimeItem runtimeItem = bag.Content.FindRuntimeItem(item);
            if (runtimeItem == null) return string.Empty;
            
            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Number.ToString("G")
                : string.Empty;
        }

        public override string String => $"{this.m_Item}[{this.m_PropertyId}] Value";
    }
}