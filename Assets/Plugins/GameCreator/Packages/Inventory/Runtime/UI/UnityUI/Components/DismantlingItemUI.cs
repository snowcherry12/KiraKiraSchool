using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Dismantling Item UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoItemUI.png")]
    
    public class DismantlingItemUI : TTinkerItemUI
    {
        [SerializeField] private PropertyGetDecimal m_RecoverChance = new PropertyGetDecimal(1f);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override bool CanTinker => Crafting.CanDismantle(
            this.RuntimeItem.Item,
            this.InputBag,
            this.OutputBag
        );

        protected override bool EnoughIngredients => true;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Dismantle()
        {
            _ = this.DoDismantle();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected override void Tinker()
        {
            _ = this.DoDismantle();
        }

        private async Task<bool> DoDismantle()
        {
            if (this.IsRunning) return false;
            if (!Crafting.CanDismantle(this.RuntimeItem.Item, this.InputBag, this.OutputBag)) return false;
            
            _ = this.m_OnStart.Run(this.Args);
            
            bool awaitSuccess = await this.WaitForTime();
            if (!awaitSuccess) return false;

            float chance = (float) this.m_RecoverChance.Get(this.Args);
            
            RuntimeItem[] runtimeItems = Crafting.Dismantle(this.RuntimeItem, this.InputBag, this.OutputBag, chance);
            if (runtimeItems == null) return false;
            
            _ = this.m_OnComplete.Run(this.Args);
            return true;
        }
    }
}