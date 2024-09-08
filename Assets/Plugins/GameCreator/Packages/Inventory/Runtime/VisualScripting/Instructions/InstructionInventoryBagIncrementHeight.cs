using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Increment Bag Height")]
    [Description("Increases the amount of rows a Bag has, if possible")]

    [Category("Inventory/Bags/Increment Bag Height")]
    
    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Rows", "The number of rows to increment by")]

    [Keywords("Bag", "Inventory", "Container", "Stash")]
    [Keywords("Column", "Size")]

    [Image(typeof(IconBagOutline), ColorTheme.Type.Yellow, typeof(OverlayArrowDown))]
    
    [Serializable]
    public class InstructionInventoryBagIncrementHeight : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetInteger m_Rows = GetDecimalInteger.Create(1);
    
        public override string Title => $"{this.m_Bag} Rows + {this.m_Rows}";
        
        protected override Task Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            if (!bag.Shape.CanIncreaseHeight) return DefaultResult;

            int amount = (int) this.m_Rows.Get(args);
            bag.Shape.IncreaseHeight(amount);

            return DefaultResult;
        }
    }
    
}
