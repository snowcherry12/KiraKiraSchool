using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Drop Runtime Item")]
    [Description("Drops a Runtime Item from its Bag onto the scene")]

    [Category("Inventory/Bags/Drop Runtime Item")]
    
    [Parameter("Runtime Item", "The instance of an Item dropped")]
    [Parameter("Distance", "The distance from the Bag where the Item is dropped")]

    [Keywords("Leave", "Eliminate", "Take")]

    [Image(typeof(IconItem), ColorTheme.Type.Blue, typeof(OverlayArrowDown))]
    
    [System.Serializable]
    public class InstructionInventoryDropRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private PropertyGetDecimal m_Distance = GetDecimalDecimal.Create(1.5f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Drop {this.m_RuntimeItem}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Item == null) return DefaultResult;
            
            if (!runtimeItem.Item.HasPrefab) return DefaultResult;
            if (!runtimeItem.Item.CanDrop) return DefaultResult;

            Bag bag = runtimeItem.Bag;
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
                distance, -1, QueryTriggerInteraction.Ignore
            );

            Vector3 position = isHit ? hit.point : rayForward.origin + rayForward.direction;
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(-360f, 360f), 0f); 
            
            Ray rayDown = new Ray(position, Vector3.down);
            isHit = Physics.Raycast(
                rayDown, out hit, 
                distance, -1, QueryTriggerInteraction.Ignore
            );
            
            position.y = isHit ? hit.point.y : rayDown.origin.y - distance;

            RuntimeItem removedRuntimeItem = bag.Content.Remove(runtimeItem);
            if (removedRuntimeItem != null) Item.Drop(runtimeItem, position, rotation);

            return DefaultResult;
        }
    }
}