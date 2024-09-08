using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    internal class FormulaHelpTool : VisualElement
    {
        internal readonly struct Parameter
        {
            public readonly string name;
            public readonly string desc;

            public Parameter(string name, string desc)
            {
                this.name = name;
                this.desc = desc;
            }
        }
        
        // CONSTANTS: -----------------------------------------------------------------------------

        private const string NAME_ROOT = "GC-Stats-Formula-Help-Root";
        private const string NAME_HEAD = "GC-Stats-Formula-Help-Head";
        private const string NAME_BODY = "GC-Stats-Formula-Help-Body";

        private const string CLASS_ITEM_NAME = "gc-stats-formula-help-name";
        private const string CLASS_ITEM_DESC = "gc-stats-formula-help-desc";
        private const string CLASS_MONOSPACE = "gc-monospace";

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly VisualElement m_Root;
        private readonly VisualElement m_Head;
        private readonly VisualElement m_Body;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private bool IsExpanded
        {
            get => EditorPrefs.GetBool($"gcstats:formula:expanded{this.Title}", true);
            set => EditorPrefs.SetBool($"gcstats:formula:expanded{this.Title}", value);
        }
        
        private string Title { get; }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FormulaHelpTool(string title, params Parameter[] parameters)
        {
            this.Title = title;

            this.m_Root = new VisualElement { name = NAME_ROOT };
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new VisualElement { name = NAME_BODY };

            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);

            Button toggle = new Button(() =>
            {
                this.IsExpanded = !this.IsExpanded;
                this.UpdateBody();
            }) { text = this.Title };
            
            this.m_Head.Add(toggle);

            foreach (Parameter parameter in parameters)
            {
                VisualElement parameterLine = new VisualElement();
                
                Label parameterName = new Label(parameter.name);
                Label parameterDesc = new Label(parameter.desc);

                parameterName.style.unityFontDefinition = default;
                parameterName.AddToClassList(CLASS_MONOSPACE);
                parameterName.AddToClassList(CLASS_ITEM_NAME);
                parameterDesc.AddToClassList(CLASS_ITEM_DESC);

                parameterName.style.color = ColorTheme.Get(ColorTheme.Type.TextNormal);
                parameterDesc.style.color = ColorTheme.Get(ColorTheme.Type.TextLight);

                parameterLine.Add(parameterName);
                parameterLine.Add(parameterDesc);
                
                this.m_Body.Add(parameterLine);
            }
            
            this.UpdateBody();
            this.Add(this.m_Root);
        }

        private void UpdateBody()
        {
            this.m_Body.style.display = this.IsExpanded
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}