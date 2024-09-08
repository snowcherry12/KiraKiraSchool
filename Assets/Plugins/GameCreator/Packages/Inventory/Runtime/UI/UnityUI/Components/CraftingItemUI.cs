using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Crafting Item UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoItemUI.png")]
    
    public class CraftingItemUI : TTinkerItemUI
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override bool CanTinker => Crafting.CanCraft(
            this.RuntimeItem.Item,
            this.InputBag,
            this.OutputBag
        );
        
        protected override bool EnoughIngredients => Crafting.EnoughCraftingIngredients(
            this.RuntimeItem.Item,
            this.InputBag
        );

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Craft()
        {
            _ = this.DoCraft();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected override void Tinker()
        {
            _ = this.DoCraft();
        }
        
        private async Task<bool> DoCraft()
        {
            if (this.IsRunning) return false;
            if (!Crafting.CanCraft(this.RuntimeItem.Item, this.InputBag, this.OutputBag)) return false;
            if (!Crafting.EnoughCraftingIngredients(this.RuntimeItem.Item, this.InputBag)) return false;
            
            _ = this.m_OnStart.Run(this.Args);
            
            bool awaitSuccess = await this.WaitForTime();
            if (!awaitSuccess) return false;
            
            RuntimeItem runtimeItem = Crafting.Craft(this.RuntimeItem.Item, this.InputBag, this.OutputBag);
            if (runtimeItem == null) return false;
            
            _ = this.m_OnComplete.Run(this.Args);
            return true;
        }
    }
}