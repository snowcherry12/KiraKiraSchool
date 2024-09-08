using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TNodeToolActionPlanConditions : TNodeToolActionPlan
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private VisualElement m_ContentBeliefs;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 250f;

        public override bool CanMove => true;
        public override bool CanDelete => true;
        
        protected override bool ShowHead => true;
        protected override bool ShowBody => true;
        protected override bool ShowFoot => true;

        protected override bool DrawConditions => false;
        protected override bool DrawInstructions => false;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        protected TNodeToolActionPlanConditions(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void CreateContent()
        {
            base.CreateContent();

            this.m_ContentBeliefs = new VisualElement { name = NAME_CONTENT_CONTENT };
            this.m_Body.Add(this.m_ContentBeliefs);
        }

        protected override void RefreshBody()
        {
            base.RefreshBody();
            
            SerializedProperty beliefs = this.Property
                .FindPropertyRelative("m_Beliefs")
                .FindPropertyRelative(BeliefsDrawer.PROP_LIST);

            int numBeliefs = beliefs.arraySize;
            
            for (int i = this.m_ContentBeliefs.childCount; i < numBeliefs; ++i)
            {
                this.m_ContentBeliefs.Add(new NodeBeliefTool());
            }

            for (int i = this.m_ContentBeliefs.childCount - 1; i >= numBeliefs; --i)
            {
                this.m_ContentBeliefs.RemoveAt(i);
            }

            for (int i = 0; i < numBeliefs; ++i)
            {
                NodeBeliefTool instance = this.m_ContentBeliefs[i] as NodeBeliefTool;
                instance?.Refresh(beliefs.GetArrayElementAtIndex(i), this);
            }
        }
    }
}