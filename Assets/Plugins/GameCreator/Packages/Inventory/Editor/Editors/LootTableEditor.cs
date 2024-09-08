using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(LootTable))]
    public class LootTableEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/LootTable";

        private const string PROP_NO_DROP_RATE = "m_NoDropRate";
        private const string PROP_LOOT_LIST = "m_LootList";

        private const string NAME_CHART_ROOT = "GC-Inventory-Loot-Table-Chart-Root";
        private const string NAME_CHART_ELEMENT = "GC-Inventory-Loot-Table-Chart-Element";

        private const string CLASS_CHART_ITEM = "gc-inventory-loot-table-chart-item";
        private const int CHART_SIZE = 300;

        internal static readonly ColorTheme.Type[] CHART_COLORS = 
        {
            ColorTheme.Type.Red,
            ColorTheme.Type.Yellow,
            ColorTheme.Type.Purple,
            ColorTheme.Type.Green,
            ColorTheme.Type.Blue,
            ColorTheme.Type.Pink
        };

        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;

        private LootListTool m_LootListTool;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();

            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);

            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            this.PaintHead();
            this.PaintBody();
            
            this.m_Root.RegisterCallback<SerializedPropertyChangeEvent>(_ =>
            {
                this.PaintHead();
                this.m_LootListTool.RefreshHeads();
            });

            return this.m_Root;
        }

        private void PaintHead()
        {
            this.m_Head.Clear();
            this.m_Head.Add(new SpaceSmall());

            VisualElement chartRoot = new VisualElement { name = NAME_CHART_ROOT };
            VisualElement chartElement = new VisualElement { name = NAME_CHART_ELEMENT };

            chartElement.style.width = CHART_SIZE;
            
            chartRoot.Add(new FlexibleSpace());
            chartRoot.Add(chartElement);
            chartRoot.Add(new FlexibleSpace());
            
            this.m_Head.Add(chartRoot);
            this.m_Head.Add(new SpaceSmall());
            
            LootTable lootTable = this.target as LootTable;
            if (lootTable == null) return;
            
            float totalRate = lootTable.TotalRate;
            for (int i = 0; i < lootTable.Loot.Length; ++i)
            {
                Loot loot = lootTable.Loot[i];
                float rate = loot.Rate;
                float percent = rate / totalRate;

                Color color = ColorTheme.Get(CHART_COLORS[i % CHART_COLORS.Length]);
                this.CreateChartElement(chartElement, color, percent);
            }
        }

        private void CreateChartElement(VisualElement chart, Color color, float percent)
        {
            VisualElement element = new VisualElement();
            
            element.AddToClassList(CLASS_CHART_ITEM);
            element.style.width = CHART_SIZE * percent;
            element.style.backgroundColor = color;
            
            chart.Add(element);
        }
        
        private void PaintBody()
        {
            SerializedProperty noDropRate = this.serializedObject.FindProperty(PROP_NO_DROP_RATE);
            SerializedProperty lootList = this.serializedObject.FindProperty(PROP_LOOT_LIST);

            PropertyField fieldNoDropRate = new PropertyField(noDropRate);
            this.m_LootListTool = new LootListTool(lootList, "m_List");
            
            this.m_Body.Add(this.m_LootListTool);
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(fieldNoDropRate);
        }
    }
}