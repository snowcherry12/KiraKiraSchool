using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomEditor(typeof(Table))]
    public class TableEditor : UnityEditor.Editor
    {
        private const string KEY_ZOOM = "gcstats:table:zoom";
        
        private const string USS_PATH = EditorPaths.PACKAGES + "Stats/Editor/StyleSheets/Table";

        private const string NAME_TABLE_ROOT = "GC-Stats-Table-Root";
        private const string NAME_TABLE_DATA = "GC-Stats-Table-Data";
        private const string NAME_TABLE_PLOT = "GC-Stats-Table-Plot";

        private const string NAME_PLOT_SCROLL = "GC-Stats-Table-Plot-Scroll";
        private const string NAME_PLOT_SCROLL_VIEW = "GC-Stats-Table-Plot-View";
        private const string NAME_PLOT_SCROLL_CONT = "GC-Stats-Table-Plot-Content";
        private const string NAME_PLOT_INFO = "GC-Stats-Table-Plot-Info";
        
        private const string CLASS_BAR_CONTENT = "gc-stats-table-bar";
        private const string CLASS_BAR_VALUE = "gc-stats-table-bar-value";
        
        private const string CLASS_PLOT_INFO_CONTENT = "gc-stats-table-plot-info-content";
        private const string CLASS_PLOT_INFO_TITLE = "gc-stats-table-plot-info-title";

        private const float PLOT_HEIGHT = 300f;

        private const float MIN_ZOOM = 3f;
        private const float MAX_ZOOM = 50f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private SerializedProperty m_PropertyTable;
        
        private VisualElement m_Root;
        private VisualElement m_Data;
        private VisualElement m_Plot;

        private ScrollView m_PlotScroll;

        private VisualElement m_PlotInfo;
        private Label m_PlotInfoLevel;
        private Label m_PlotInfoStep;
        private Label m_PlotInfoRange;
        
        private TTable m_Table;

        // PROPERTIES: ----------------------------------------------------------------------------

        private static float Zoom
        {
            get => EditorPrefs.GetFloat(KEY_ZOOM, 2f);
            set => EditorPrefs.SetFloat(KEY_ZOOM, Mathf.Clamp(value, MIN_ZOOM, MAX_ZOOM));
        }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override bool UseDefaultMargins() => false;
        
        // PAINT METHOD: --------------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement { name = NAME_TABLE_ROOT };
            this.m_Data = new VisualElement { name = NAME_TABLE_DATA };
            this.m_Plot = new VisualElement { name = NAME_TABLE_PLOT };
            
            this.m_Root.Add(this.m_Plot);
            this.m_Root.Add(this.m_Data);

            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            this.m_PropertyTable = this.serializedObject.FindProperty("m_Table");

            this.InitializeData();
            this.InitializePlot();

            return this.m_Root;
        }

        private void InitializeData()
        {
            TableElementTool elementTable = new TableElementTool(this.m_PropertyTable);
            
            elementTable.EventChangeType += (_, newType) =>
            {
                this.m_PropertyTable.serializedObject.Update();
                this.m_Table = this.m_PropertyTable.GetValue<TTable>();
                
                if (this.m_Table != null) this.MakePlot();
            };

            elementTable.EventChangeValue += property =>
            {
                this.m_PropertyTable.serializedObject.Update();
                this.m_Table = this.m_PropertyTable.GetValue<TTable>();
                
                if (this.m_Table != null) this.MakePlot();
            };
            
            this.m_Data.Add(elementTable);
        }

        private void InitializePlot()
        {
            this.m_Table = this.m_PropertyTable.GetValue<TTable>();
            if (this.m_Table != null) this.MakePlot();
        }
        
        // PLOT PAINTING: -------------------------------------------------------------------------

        private void MakePlot()
        {
            this.m_Plot.Clear();
            this.m_PlotScroll = new ScrollView(ScrollViewMode.Horizontal)
            {
                name = NAME_PLOT_SCROLL
            };

            this.m_PlotScroll.horizontalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
            this.m_PlotScroll.contentViewport.name = NAME_PLOT_SCROLL_VIEW;
            this.m_PlotScroll.contentContainer.name = NAME_PLOT_SCROLL_CONT;

            this.m_PlotInfo = new VisualElement
            {
                name = NAME_PLOT_INFO, 
                focusable = false, 
                pickingMode = PickingMode.Ignore
            };

            VisualElement plotInfoContentLevel = new VisualElement();
            VisualElement plotInfoContentStep = new VisualElement();
            VisualElement plotInfoContentRange = new VisualElement();
            
            plotInfoContentLevel.AddToClassList(CLASS_PLOT_INFO_CONTENT);
            plotInfoContentStep.AddToClassList(CLASS_PLOT_INFO_CONTENT);
            plotInfoContentRange.AddToClassList(CLASS_PLOT_INFO_CONTENT);

            Label labelLevel = new Label("<b>Level:</b>") { pickingMode = PickingMode.Ignore };
            Label labelStep = new Label("Step:") { pickingMode = PickingMode.Ignore };
            Label labelRange = new Label("Range:") { pickingMode = PickingMode.Ignore };
            
            labelLevel.AddToClassList(CLASS_PLOT_INFO_TITLE);
            labelStep.AddToClassList(CLASS_PLOT_INFO_TITLE);
            labelRange.AddToClassList(CLASS_PLOT_INFO_TITLE);

            this.m_PlotInfoLevel = new Label { pickingMode = PickingMode.Ignore };
            this.m_PlotInfoStep  = new Label { pickingMode = PickingMode.Ignore };
            this.m_PlotInfoRange = new Label { pickingMode = PickingMode.Ignore };
            
            plotInfoContentLevel.Add(labelLevel);
            plotInfoContentLevel.Add(this.m_PlotInfoLevel);
            
            plotInfoContentStep.Add(labelStep);
            plotInfoContentStep.Add(this.m_PlotInfoStep);
            
            plotInfoContentRange.Add(labelRange);
            plotInfoContentRange.Add(this.m_PlotInfoRange);

            this.m_PlotInfo.Add(plotInfoContentLevel);
            this.m_PlotInfo.Add(plotInfoContentStep);
            this.m_PlotInfo.Add(plotInfoContentRange);

            this.m_PlotInfo.style.display = DisplayStyle.None;

            this.MakePlotContent();
            this.m_PlotScroll.contentContainer.style.height = PLOT_HEIGHT;

            Slider zoomSlider = new Slider("Zoom", MIN_ZOOM, MAX_ZOOM) { value = Zoom };

            zoomSlider.RegisterValueChangedCallback(changeEvent =>
            {
                Zoom = changeEvent.newValue;
                this.MakePlotContent();
            });

            this.m_Plot.Add(zoomSlider);
            this.m_Plot.Add(this.m_PlotScroll);
            this.m_Plot.Add(this.m_PlotInfo);
        }

        private void MakePlotContent()
        {
            this.m_PlotScroll.contentContainer.Clear();
            for (int i = 1; i <= this.m_Table.MaxLevel; ++i)
            {
                VisualElement element = this.MakePlotBar(i);
                this.m_PlotScroll.contentContainer.Add(element);
            }
        }
        
        private VisualElement MakePlotBar(int index)
        {
            VisualElement barContent = new VisualElement();
            VisualElement barValue = new VisualElement();
            
            barContent.AddToClassList(CLASS_BAR_CONTENT);
            barValue.AddToClassList(CLASS_BAR_VALUE);
            
            barContent.style.flexBasis = Zoom + 0.5f;

            float maxExperience = this.m_Table.GetCumulativeExperienceForLevel(this.m_Table.MaxLevel);
            float experience = this.m_Table.GetLevelExperienceForLevel(index);
            float expRangeA = this.m_Table.GetCumulativeExperienceForLevel(index);
            float expRangeB = this.m_Table.GetCumulativeExperienceForLevel(index + 1) - 1;

            float height = Mathf.Lerp(0, PLOT_HEIGHT, expRangeA / maxExperience);
            float marginTop = PLOT_HEIGHT - height;
            
            barValue.style.height = height;
            barValue.style.marginTop = marginTop;
            barValue.style.backgroundColor = ColorTheme.Get(ColorTheme.Type.Green);
            
            barContent.RegisterCallback<MouseOverEvent>(overEvent =>
            {
                this.m_PlotInfo.style.display = DisplayStyle.Flex;
                this.m_PlotInfoLevel.text = $"<b>{index}</b>";
                this.m_PlotInfoStep.text = $"{experience}";
                this.m_PlotInfoRange.text = $"{expRangeA} - {expRangeB}";
            });
            
            barContent.Add(barValue);
            
            return barContent;
        }
    }
}