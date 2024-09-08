using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Behavior
{
    public class BeliefTool : TPolymorphicItemTool
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Belief-Head",
            EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Belief-Body"
        };
        
        protected override object Value => this.m_Property.GetValue<Belief>();

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public BeliefTool(IPolymorphicListTool parentTool, int index) 
            : base(parentTool, index)
        { }
    }
}