using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Tinker Skin",
        menuName = "Game Creator/Developer/Inventory/Tinker Skin",
        order = 300
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoTinker.png")]
    
    public class TinkerSkin : TSkin<GameObject>
    {
        private const string MSG = "A game object prefab with UI components for Crafting and Dismantling";
        
        private const string ERR_NO_VALUE = "Prefab value cannot be empty";
        private const string ERR_BAG_UI = "Prefab does not contain a 'TinkerUI' component";

        public override string Description => MSG;

        public override string HasError
        {
            get
            {
                if (this.Value == null) return ERR_NO_VALUE;
                return !this.Value.Get<TinkerUI>() ? ERR_BAG_UI : string.Empty;
            }
        }
        
        private GameObject SceneInstance { get; set; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OpenUI(Bag inputBag, Bag outputBag, Item filterItem = null)
        {
            if (inputBag == null) return;
            if (outputBag == null) return;

            if (this.SceneInstance == null)
            {
                if (this.Value == null) return;
                this.SceneInstance = Instantiate(this.Value);    
            }
            
            TinkerUI tinkerUI = this.SceneInstance.Get<TinkerUI>();
            if (tinkerUI == null) return;

            tinkerUI.OpenUI(inputBag, outputBag, filterItem);
            this.SceneInstance.SetActive(true);
        }
    }
}