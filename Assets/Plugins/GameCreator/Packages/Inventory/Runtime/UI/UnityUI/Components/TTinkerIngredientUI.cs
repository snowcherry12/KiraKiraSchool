using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoTooltipUI.png")]
    
    public abstract class TTinkerIngredientUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private ItemUI m_IngredientUI = new ItemUI();

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public virtual void RefreshUI(Bag bag, Ingredient ingredient)
        {
            this.m_IngredientUI.RefreshUI(bag, ingredient.Item, true);

            int amountRequired = ingredient.Amount;
            int amountInBag = bag.Content.CountType(ingredient.Item);
        }
    }
}