using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolActionPlanPreConditions : TNodeToolActionPlanConditions
    {
        private static readonly IIcon ICON = new IconNodePreConditions(ColorTheme.Type.Yellow);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => "Requisites";

        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolActionPlanPreConditions(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}