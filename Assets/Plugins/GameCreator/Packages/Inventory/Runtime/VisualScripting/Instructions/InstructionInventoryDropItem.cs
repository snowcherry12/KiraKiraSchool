using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Drop Item")]
    [Description("Drops an Item type from a Bag onto the scene")]

    [Category("Inventory/Bags/Drop Item")]
    
    [Parameter("Item", "The type of item created")]
    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Distance", "The distance from the Bag where the Item is dropped")]

    [Keywords("Leave", "Eliminate", "Take")]

    [Image(typeof(IconItem), ColorTheme.Type.Green, typeof(OverlayArrowDown))]
    
    [System.Serializable]
    public class InstructionInventoryDropItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetDecimal m_Distance = GetDecimalDecimal.Create(1.5f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Drop {this.m_Item} from {this.m_Bag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            if (item == null) return DefaultResult;
            
            if (!item.HasPrefab) return DefaultResult;
            if (!item.CanDrop) return DefaultResult;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            GameObject origin = bag.Wearer;
            if (origin == null) origin = bag.gameObject;

            float distance = (float) this.m_Distance.Get(args);
            Vector2 randomCircle = Random.insideUnitCircle;
            
            Ray rayForward = new Ray(
                origin.transform.position,
                new Vector3(randomCircle.x, 0f, randomCircle.y)
            );

            bool isHit = Physics.Raycast(
                rayForward, out RaycastHit hit, 
                distance, item.DropLayerMask, QueryTriggerInteraction.Ignore
            );

            Vector3 position = isHit ? hit.point : rayForward.origin + rayForward.direction;
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(-360f, 360f), 0f); 
            
            Ray rayDown = new Ray(position, Vector3.down);
            isHit = Physics.Raycast(
                rayDown, out hit, 
                distance, item.DropLayerMask, QueryTriggerInteraction.Ignore
            );
            
            position.y = isHit ? hit.point.y : rayDown.origin.y - distance;

            RuntimeItem runtimeItem = bag.Content.RemoveType(item);
            if (runtimeItem != null) Item.Drop(runtimeItem, position, rotation);

            return DefaultResult;
        }
    }
}