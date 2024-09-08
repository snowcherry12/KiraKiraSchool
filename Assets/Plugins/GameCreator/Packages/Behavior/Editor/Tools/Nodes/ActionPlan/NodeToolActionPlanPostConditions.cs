using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolActionPlanPostConditions : TNodeToolActionPlanConditions
    {
        private static readonly IIcon ICON = new IconNodePostConditions(ColorTheme.Type.Green);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => "Effects";

        public override Texture Icon => ICON.Texture;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolActionPlanPostConditions(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}