using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Increment Bag Width")]
    [Description("Increases the amount of columns a Bag has, if possible")]

    [Category("Inventory/Bags/Increment Bag Width")]
    
    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Columns", "The number of columns to increment by")]

    [Keywords("Bag", "Inventory", "Container", "Stash")]
    [Keywords("Column", "Size")]

    [Image(typeof(IconBagOutline), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionInventoryBagIncrementWidth : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetInteger m_Columns = GetDecimalInteger.Create(1);

        public override string Title => $"{this.m_Bag} Columns + {this.m_Columns}";

        protected override Task Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            if (!bag.Shape.CanIncreaseWidth) return DefaultResult;

            int amount = (int) this.m_Columns.Get(args);
            bag.Shape.IncreaseWidth(amount);

            return DefaultResult;
        }
    }
    
}
