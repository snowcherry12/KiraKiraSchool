using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ConnectionTool : VisualElement
    {
        private static readonly IIcon ICON = new IconArrowRight(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly TGraphTool m_GraphTool;
        private readonly Label m_LabelIndex;
        private readonly Label m_LabelNode;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ConnectionTool(TGraphTool graphTool)
        {
            this.m_GraphTool = graphTool;
            this.m_LabelIndex = new Label();
            this.m_LabelNode = new Label();
            
            this.Add(this.m_LabelIndex);
            this.Add(new Image { image = ICON.Texture });
            this.Add(this.m_LabelNode);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(SerializedProperty connectionsProperty, int index)
        {
            string connectionId = connectionsProperty
                .GetArrayElementAtIndex(index)
                .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                .stringValue;
            
            string node = this.m_GraphTool.PortTools.TryGetValue(connectionId, out TPortTool port)
                ? port.NodeTool.Title
                : connectionId;

            this.m_LabelIndex.text = (index + 1).ToString();
            this.m_LabelNode.text = node;
        }
    }
}