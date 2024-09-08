using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Set Bag UI")]
    [Description("Changes the targeted Bag of a Bag UI component")]

    [Category("Inventory/UI/Set Bag UI")]
    
    [Parameter("Bag UI", "The Bag UI that changes its target")]
    [Parameter("Bag", "The new Bag component")]

    [Image(typeof(IconBagSolid), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionInventorySetBagUI : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private TBagUI m_BagUI;
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Set {0} to {1}",
            this.m_BagUI != null ? this.m_BagUI.gameObject.name : "(none)",
            this.m_Bag
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (this.m_BagUI == null) return DefaultResult;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            this.m_BagUI.ChangeBag(bag, false);
            return DefaultResult;
        }
    }
}