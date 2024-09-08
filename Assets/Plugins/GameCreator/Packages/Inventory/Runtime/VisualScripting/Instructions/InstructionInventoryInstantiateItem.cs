using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Instantiate Item")]
    [Description("Instantiates the prefab of an item on the scene")]

    [Category("Inventory/Loot/Instantiate Item")]
    
    [Parameter("Item", "The type of item created")]
    [Parameter("Location", "The position and rotation where the item instance is placed")]

    [Keywords("Drop", "Inventory", "Instance")]

    [Image(typeof(IconItem), ColorTheme.Type.Blue, typeof(OverlayArrowUp))]
    
    [Serializable]
    public class InstructionInventoryInstantiateItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] 
        private PropertyGetItem m_Item = new PropertyGetItem();

        [SerializeField]
        private PropertyGetLocation m_Location = GetLocationPositionRotation.Create(
            GetPositionCharactersPlayer.Create,
            GetRotationCharactersPlayer.Create
        );

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Instantiate {this.m_Item} at {this.m_Location}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            if (item == null) return DefaultResult;

            Location location = this.m_Location.Get(args);
            GameObject user = args.Self;

            RuntimeItem runtimeItem = item.CreateRuntimeItem(args);
            
            Item.Instantiate(
                runtimeItem,
                location.GetPosition(user),
                location.GetRotation(user)
            );
            
            return DefaultResult;
        }
    }
}