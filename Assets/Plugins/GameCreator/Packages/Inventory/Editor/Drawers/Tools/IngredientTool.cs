using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public class IngredientTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_INGREDIENT = new IconIngredient(ColorTheme.Type.Green);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Ingredient-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Ingredient-Body",
        };

        public override string Title
        {
            get
            {
                SerializedProperty item = this.m_Property
                    .FindPropertyRelative(IngredientDrawer.PROP_ITEM);

                SerializedProperty amount = this.m_Property
                    .FindPropertyRelative(IngredientDrawer.PROP_AMOUNT);

                string itemName = item.objectReferenceValue != null
                    ? item.objectReferenceValue.name
                    : "(none)";
                
                return $"{itemName} x{amount.intValue}";
            }
        }

        protected override Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public IngredientTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_INGREDIENT.Texture;
        }
    }
}