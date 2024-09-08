using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class NodeConditionTool : VisualElement
    {
        private static readonly IIcon ICON_BREAKPOINT = new IconBreakpoint(ColorTheme.Type.Red);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly Image m_Icon;
        private readonly Label m_Text;
        private readonly Image m_Breakpoint;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public NodeConditionTool()
        {
            this.m_Icon = new Image();
            this.m_Text = new Label();
            this.m_Breakpoint = new Image { image = ICON_BREAKPOINT.Texture };
            
            this.Add(this.m_Icon);
            this.Add(this.m_Text);
            this.Add(this.m_Breakpoint);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh(SerializedProperty propertyInstruction)
        {
            Condition instance = propertyInstruction.managedReferenceValue as Condition;
                
            IEnumerable<ImageAttribute> iconAttrs = instance?.GetType()
                .GetCustomAttributes<ImageAttribute>();
            Texture2D icon = iconAttrs?.FirstOrDefault()?.Image;

            this.m_Icon.image = icon != null ? icon : Texture2D.whiteTexture; 
            this.m_Text.text = instance?.Title;

            bool isEnabled = propertyInstruction.FindPropertyRelative("m_IsEnabled").boolValue;
            
            this.m_Icon.style.opacity = isEnabled ? 1f: 0.35f;
            this.m_Text.style.opacity = isEnabled ? 1f: 0.35f;
            
            this.m_Breakpoint.style.display = propertyInstruction.FindPropertyRelative("m_Breakpoint").boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}