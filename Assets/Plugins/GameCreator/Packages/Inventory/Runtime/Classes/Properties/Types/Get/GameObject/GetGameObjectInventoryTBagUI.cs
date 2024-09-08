using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("From Bag UI")]
    [Category("Inventory/From Bag UI")]
    
    [Image(typeof(IconBagOutline), ColorTheme.Type.Yellow)]
    [Description("The Bag assigned to a TBagUI component")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryTBagUI : PropertyTypeGetGameObject
    {
        [SerializeField] protected TBagUI m_BagUI;

        public override GameObject Get(Args args)
        {
            return this.GetValue();
        }

        public override GameObject Get(GameObject gameObject)
        {
            return this.GetValue();
        }
        
        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(TBagUI)) return this.m_BagUI as T;
            return base.Get<T>(args);
        }
        
        public static PropertyGetGameObject Create()
        {
            GetGameObjectInventoryTBagUI instance = new GetGameObjectInventoryTBagUI();
            return new PropertyGetGameObject(instance);
        }
        
        public override GameObject EditorValue => this.m_BagUI != null
            ? this.m_BagUI.gameObject
            : null;

        private GameObject GetValue()
        {
            if (this.m_BagUI == null) return null;
            return this.m_BagUI.Bag != null ? this.m_BagUI.Bag.gameObject : null;
        }

        public override string String => this.m_BagUI != null
            ? this.m_BagUI.gameObject.name
            : "(none)";
    }
}