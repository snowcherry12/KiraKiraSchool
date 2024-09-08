using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TNodeToolUtilityBoard : TNodeTool
    {
        private const string PROP_SCORE = "m_Score";

        private const float MARGIN_V = 8f;
        private const float MARGIN_H = 3f;

        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_ContentScore;

        private Label m_ValueField;
        private CurveField m_ScoreField;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        protected TNodeToolUtilityBoard(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override string GetPortText(string portId) => string.Empty;
        
        protected override TPortTool CreatePort(TNodeTool nodeTool, SerializedProperty property)
        {
            return null;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void CreateContent()
        {
            this.m_ContentScore = new VisualElement { name = NAME_CONTENT_CONTENT };
            VisualElement separator = new VisualElement { name = NAME_CONTENT_SEPARATOR };

            this.m_ValueField = new Label
            {
                style =
                {
                    marginTop = MARGIN_V,
                    marginLeft = MARGIN_H,
                    marginRight = MARGIN_H
                }
            };

            this.m_ScoreField = ScoreDrawer.CreateCurveField();
            this.m_ScoreField.pickingMode = PickingMode.Ignore;
            this.m_ScoreField.delegatesFocus = false;
            this.m_ScoreField.focusable = false;
            this.m_ScoreField.style.marginBottom = MARGIN_V;
            this.m_ScoreField.style.marginTop = MARGIN_V;
            this.m_ScoreField.style.marginLeft = MARGIN_H;
            this.m_ScoreField.style.marginRight = MARGIN_H;
            this.m_ScoreField.SetEnabled(false);

            this.m_ContentScore.Add(this.m_ValueField);
            this.m_ContentScore.Add(this.m_ScoreField);
            
            this.m_Body.Add(this.m_ContentScore);
            this.m_Body.Add(separator);

            base.CreateContent();
        }

        protected override void RefreshBody()
        {
            this.m_ValueField.text = this.GetTitle();
            this.m_ValueField.style.opacity = TargetUtils.Processor != null ? 1f : 0.5f;
            
            SerializedProperty propertyCurve = this.Property
                .FindPropertyRelative(PROP_SCORE)
                .FindPropertyRelative(ScoreDrawer.PROP_CURVE);

            this.m_ScoreField.bindingPath = propertyCurve.propertyPath;
            this.m_ScoreField.BindProperty(propertyCurve);
            
            base.RefreshBody();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private string GetTitle()
        {
            if (this.Node is not TNodeUtilityBoard node) return string.Empty;
            string title = node.ScoreTitle;
            
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return title;

            Processor processor = TargetUtils.Processor;
            if (processor == null) return title;

            IValueWithScore value = node.GetValue<IValueWithScore>(processor);
            return value != null ? $"{title} = {value.Score}" : title;
        }
    }
}