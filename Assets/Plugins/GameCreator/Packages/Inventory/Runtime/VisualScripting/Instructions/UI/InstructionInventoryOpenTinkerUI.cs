using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Open Tinker UI")]
    [Description("Opens an Tinkering UI for a specific Bag")]

    [Category("Inventory/UI/Open Tinker UI")]
    
    [Parameter("Tinker Skin", "The skin that is used to display the UI")]
    [Parameter("Input Bag", "The Bag component where items are chosen")]
    [Parameter("Output Bag", "The Bag component where new items are placed")]
    [Parameter("Wait to Close", "If the Instruction waits until the UI closes")]
    
    [Keywords("Craft", "Make", "Create", "Dismantle", "Disassemble", "Torn")]
    [Keywords("Alchemy", "Blacksmith")]

    [Image(typeof(IconCraft), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionInventoryOpenTinkerUI : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private TinkerSkin m_TinkerSkin;

        [SerializeField] private PropertyGetGameObject m_InputBag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetGameObject m_OutputBag = GetGameObjectPlayer.Create();

        [SerializeField] private Item m_FilterItem;
        [SerializeField] private bool m_WaitToClose;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Open {0} with {1}{2}",
            this.m_TinkerSkin != null ? this.m_TinkerSkin.name : "(none)",
            this.m_InputBag,
            this.m_WaitToClose ? " and wait" : string.Empty
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override async Task Run(Args args)
        {
            if (this.m_TinkerSkin == null) return;
            
            Bag inputBag = this.m_InputBag.Get<Bag>(args);
            Bag outputBag = this.m_OutputBag.Get<Bag>(args);

            this.m_TinkerSkin.OpenUI(inputBag, outputBag, this.m_FilterItem);
            await this.While(() => this.m_WaitToClose && TinkerUI.IsOpen);
        }
    }
}