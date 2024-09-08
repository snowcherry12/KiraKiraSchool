using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Dismantling Ingredient UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoIngredientUI.png")]
    
    public class DismantlingIngredientUI : TTinkerIngredientUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Text m_AmountToRetrieve;
        [SerializeField] private Text m_AmountInBag;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void RefreshUI(Bag bag, Ingredient ingredient)
        {
            base.RefreshUI(bag, ingredient);
            
            int amountInBag = bag.Content.CountType(ingredient.Item);
            int amountRetrieve = ingredient.Amount;
            
            if (this.m_AmountToRetrieve != null)
            {
                this.m_AmountToRetrieve.text = amountRetrieve.ToString();
            }
            
            if (this.m_AmountInBag != null)
            {
                this.m_AmountInBag.text = amountInBag.ToString();
            }
        }
    }
}