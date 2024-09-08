using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Open Merchant UI")]
    [Description("Opens a trading window for a specific Merchant")]

    [Category("Inventory/UI/Open Merchant UI")]
    
    [Parameter("Merchant", "The currency type to modify")]
    [Parameter("Client Bag", "The client's Bag component")]
    [Parameter("Wait to Close", "If the Instruction waits until the UI closes")]
    
    [Keywords("Trade", "Merchant", "Shop", "Buy", "Sell", "Junk")]

    [Image(typeof(IconMerchant), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionInventoryOpenMerchantUI : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Merchant = GetGameObjectInventoryMerchant.Create();
        
        [SerializeField]
        private PropertyGetGameObject m_ClientBag = GetGameObjectPlayer.Create();

        [SerializeField] private bool m_WaitToClose;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Open {0} with {1}{2}",
            this.m_Merchant,
            this.m_ClientBag,
            this.m_WaitToClose ? " and wait" : string.Empty
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            Merchant merchant = this.m_Merchant.Get<Merchant>(args);
            Bag clientBag = this.m_ClientBag.Get<Bag>(args);
            
            if (merchant == null) return;
            if (clientBag == null) return;

            merchant.OpenUI(clientBag);
            await this.While(() => this.m_WaitToClose && MerchantUI.IsOpen);
        }
    }
}