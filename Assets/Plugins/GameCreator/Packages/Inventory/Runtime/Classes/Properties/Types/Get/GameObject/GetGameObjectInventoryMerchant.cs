using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Merchant")]
    [Category("Inventory/Merchant")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple)]
    [Description("A Merchant component attached to a scene Game Object or prefab")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryMerchant : PropertyTypeGetGameObject
    {
        [SerializeField] protected Merchant m_Merchant;

        public override GameObject Get(Args args) => this.m_Merchant != null 
            ? this.m_Merchant.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => this.m_Merchant != null 
            ? this.m_Merchant.gameObject 
            : null;
        
        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(Merchant)) return this.m_Merchant as T;
            return base.Get<T>(args);
        }
        
        public GetGameObjectInventoryMerchant() : base()
        { }

        public GetGameObjectInventoryMerchant(GameObject bag) : this()
        {
            this.m_Merchant = bag != null ? bag.Get<Merchant>() : null;
        }

        public override GameObject EditorValue => this.m_Merchant != null
            ? this.m_Merchant.gameObject
            : null;

        public static PropertyGetGameObject Create()
        {
            GetGameObjectInventoryBag instance = new GetGameObjectInventoryBag();
            return new PropertyGetGameObject(instance);
        }

        public override string String => this.m_Merchant != null
            ? this.m_Merchant.gameObject.name
            : "(none)";
    }
}