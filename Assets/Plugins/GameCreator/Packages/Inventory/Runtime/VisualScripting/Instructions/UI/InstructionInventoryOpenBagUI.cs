using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Open Bag UI")]
    [Description("Opens an inventory UI of a specific Bag")]

    [Category("Inventory/UI/Open Bag UI")]
    
    [Parameter("Bag", "The Bag component")]
    [Parameter("Wait to Close", "If the Instruction waits until the UI closes")]
    
    [Keywords("Item", "Inventory", "Catalogue", "Content", "Sort")]
    [Keywords("Equipment", "Hotbar", "Consume")]

    [Image(typeof(IconBagSolid), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionInventoryOpenBagUI : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        [SerializeField] private bool m_WaitToClose;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Open {0}{1}",
            this.m_Bag,
            this.m_WaitToClose ? " and wait" : string.Empty
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;

            bag.OpenUI();
            await this.While(() => this.m_WaitToClose && TBagUI.IsOpen);
        }
    }
}