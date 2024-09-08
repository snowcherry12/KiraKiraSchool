using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Behavior
{
    public class ThoughtTool : TPolymorphicItemTool
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Thought-Head",
            EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Thought-Body"
        };
        
        protected override object Value => this.m_Property.GetValue<Thought>();

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ThoughtTool(IPolymorphicListTool parentTool, int index) 
            : base(parentTool, index)
        { }
    }
}