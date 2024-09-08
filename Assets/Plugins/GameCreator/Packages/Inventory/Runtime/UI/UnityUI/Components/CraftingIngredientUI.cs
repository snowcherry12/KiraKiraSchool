using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Crafting Ingredient UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoIngredientUI.png")]
    
    public class CraftingIngredientUI : TTinkerIngredientUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_ActiveIfEnough;
        [SerializeField] private Text m_AmountRequired;
        [SerializeField] private Text m_AmountInBag;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void RefreshUI(Bag bag, Ingredient ingredient)
        {
            base.RefreshUI(bag, ingredient);

            int amountRequired = ingredient.Amount;
            int amountInBag = bag.Content.CountType(ingredient.Item);
            
            if (this.m_ActiveIfEnough != null)
            {
                bool enough = amountInBag >= amountRequired;
                m_ActiveIfEnough.SetActive(enough);
            }
            
            if (this.m_AmountRequired != null)
            {
                this.m_AmountRequired.text = amountRequired.ToString();
            }
            
            if (this.m_AmountInBag != null)
            {
                this.m_AmountInBag.text = amountInBag.ToString();
            }
        }
    }
}