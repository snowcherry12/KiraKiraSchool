using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Property Text")]
    [Category("Inventory/Runtime Item Property Text")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Blue)]
    [Description("Sets the property Text of the Runtime Item")]
    
    [Serializable]
    public class SetStringRuntimeItemPropertyText : PropertyTypeSetString
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override string Get(Args args) => this.GetText(args);

        public override void Set(string value, Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;

            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
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
            
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return string.Empty;
            
            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Text
                : string.Empty;
        }

        public override string String => $"{this.m_RuntimeItem}[{this.m_PropertyId}] Text";
    }
}