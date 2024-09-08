using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class NodeBeliefTool : VisualElement
    {
        private static readonly IIcon ICON_BELIEF_PRE = new IconBelief(ColorTheme.Type.Yellow);
        private static readonly IIcon ICON_BELIEF_POS = new IconBelief(ColorTheme.Type.Green);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly Image m_Icon;
        private readonly Label m_Text;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public NodeBeliefTool()
        {
            this.m_Icon = new Image();
            this.m_Text = new Label();
            
            this.Add(this.m_Icon);
            this.Add(this.m_Text);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(SerializedProperty propertyBeliefs, TNodeTool nodeTool)
        {
            this.m_Icon.image = nodeTool is NodeToolActionPlanPreConditions
                ? ICON_BELIEF_PRE.Texture
                : ICON_BELIEF_POS.Texture;

            Belief instance = propertyBeliefs.managedReferenceValue as Belief;
            this.m_Text.text = instance?.Title;
        }
    }
}