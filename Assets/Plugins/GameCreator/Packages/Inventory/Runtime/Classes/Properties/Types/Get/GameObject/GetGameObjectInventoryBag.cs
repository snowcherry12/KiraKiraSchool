using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Bag")]
    [Category("Inventory/Bag")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow)]
    [Description("A Bag component attached to a scene Game Object or prefab")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryBag : PropertyTypeGetGameObject
    {
        [SerializeField] protected Bag m_Bag;

        public override GameObject Get(Args args) => this.m_Bag != null 
            ? this.m_Bag.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => this.m_Bag != null 
            ? this.m_Bag.gameObject 
            : null;
        
        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(Bag)) return this.m_Bag as T;
            return base.Get<T>(args);
        }
        
        public GetGameObjectInventoryBag() : base()
        { }

        public GetGameObjectInventoryBag(GameObject bag) : this()
        {
            this.m_Bag = bag != null ? bag.Get<Bag>() : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectInventoryBag instance = new GetGameObjectInventoryBag();
            return new PropertyGetGameObject(instance);
        }

        public override GameObject EditorValue => this.m_Bag != null
            ? this.m_Bag.gameObject
            : null;

        public override string String => this.m_Bag != null
            ? this.m_Bag.gameObject.name
            : "(none)";
    }
}