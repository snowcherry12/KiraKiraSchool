using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    public class LootTool : TPolymorphicItemTool
    {
        private const string NAME_LABEL_RATE = "GC-Inventory-Loot-Table-List-Item";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private Label m_LabelRatePercent;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Loot-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Loot-Body",
        };

        public override string Title
        {
            get
            {
                SerializedProperty type = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_TYPE);
                
                SerializedProperty amount = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_AMOUNT);
                
                SerializedProperty item = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_ITEM);
                
                SerializedProperty currency = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_CURRENCY);

                string lootName = type.enumValueIndex switch
                {
                    0 => item.objectReferenceValue != null
                        ? item.objectReferenceValue.name
                        : "(none)",
                    1 => currency.objectReferenceValue != null
                        ? currency.objectReferenceValue.name
                        : "(none)",
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                string operation = type.enumValueIndex switch
                {
                    0 => "x",
                    1 => "+",
                    _ => throw new ArgumentOutOfRangeException()
                };

                SerializedProperty amountConstant = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_AMOUNT_CONSTANT);
                
                SerializedProperty amountMin = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_AMOUNT_MIN);
                
                SerializedProperty amountMax = this.m_Property
                    .FindPropertyRelative(LootDrawer.PROP_AMOUNT_MAX);

                string amountName = amount.intValue switch
                {
                    0 => amountConstant.intValue.ToString(),
                    1 => $"[{amountMin.intValue}, {amountMax.intValue}]",
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                return $"{lootName} {operation}{amountName}";
            }
        }

        protected override Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public LootTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            int colorIndex = this.Index % LootTableEditor.CHART_COLORS.Length;
            ColorTheme.Type color = LootTableEditor.CHART_COLORS[colorIndex];

            SerializedProperty type = this.m_Property
                .FindPropertyRelative(LootDrawer.PROP_TYPE);

            return type.enumValueIndex switch
            {
                0 => new IconItem(color).Texture,
                1 => new IconCurrency(color).Texture,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected override void UpdateHead()
        {
            base.UpdateHead();
            this.RefreshHead();
        }

        protected override void SetupHeadExtras()
        {
            base.SetupHeadExtras();
            
            this.m_LabelRatePercent = new Label{ name = NAME_LABEL_RATE };
            m_HeadButton.Add(this.m_LabelRatePercent);   
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshHead()
        {
            LootTable lootTable = this.m_Property.serializedObject.targetObject as LootTable;
            if (lootTable == null) return;
            
            float rate = this.m_Property
                .FindPropertyRelative(LootDrawer.PROP_RATE)
                .intValue;

            int totalRate = lootTable.TotalRate;
            float ratePercent = totalRate > 0 ? rate / totalRate : 0f;
            this.m_LabelRatePercent.text = ratePercent.ToString("P");
        }
    }
}