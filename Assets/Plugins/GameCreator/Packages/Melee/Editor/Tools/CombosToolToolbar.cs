using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    public class CombosToolToolbar : VisualElement
    {
        private static readonly IIcon ICON_SKILL = new IconMeleeSkill(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_SUB_SKILL = new IconMeleeSubSkill(ColorTheme.Type.TextNormal);
        
        private static readonly IIcon ICON_INSPECTOR_ON = new IconSidebar(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_INSPECTOR_OFF = new IconSidebar(ColorTheme.Type.TextLight);

        private const string TIP_SKILL = "Add new Skill";
        private const string TIP_SUB_SKILL = "Add new Sub-Skill";
        private const string TIP_INSPECTOR = "Toggle Inspector";

        private const string NAME_SEPARATOR = "GC-Melee-Toolbar-Separator";
        private const string NAME_SPACE = "GC-Melee-Toolbar-Space";

        private const string CLASS_BUTTON = "gc-melee-toolbar-button";

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Button m_BtnCombo;
        [NonSerialized] private readonly Button m_BtnSubCombo;
        [NonSerialized] private readonly Button m_BtnToggleInspector;

        [NonSerialized] private readonly DropdownField m_DropdownInsertMode;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private CombosTool CombosTool { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public CombosToolToolbar(CombosTool combosTool)
        {
            this.CombosTool = combosTool;
            
            this.m_BtnCombo = this.CreateButton(
                this.AddCombo, 
                ICON_SKILL,
                TIP_SKILL, 
                CLASS_BUTTON
            );
            
            this.m_BtnSubCombo = this.CreateButton(
                this.AddSubCombo, 
                ICON_SUB_SKILL,
                TIP_SUB_SKILL, 
                CLASS_BUTTON
            );
            
            this.m_BtnToggleInspector = this.CreateButton(
                this.ToggleInspector, 
                ICON_INSPECTOR_OFF,
                TIP_INSPECTOR, 
                CLASS_BUTTON
            );

            this.Add(new Label("Insert"));
            this.Add(this.CreateSpace());
            this.Add(this.m_BtnCombo);
            this.Add(this.CreateSpace());
            this.Add(new Label("or"));
            this.Add(this.CreateSpace());
            this.Add(this.m_BtnSubCombo);
            this.Add(new FlexibleSpace());
            this.Add(this.CreateSeparator());
            this.Add(this.m_BtnToggleInspector);
        }

        public void Setup()
        {
            this.CombosTool.Inspector.EventState += this.OnChangeInspector;
            this.OnChangeInspector();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void AddCombo()
        {
            ComboItem newNode = new ComboItem();
            this.CombosTool.Tree.CreateAsSelectionSibling(newNode);
        }
        
        private void AddSubCombo()
        {
            ComboItem newNode = new ComboItem();
            this.CombosTool.Tree.CreateAsSelectionChild(newNode);
        }

        private void ToggleInspector()
        {
            this.CombosTool.Inspector.ToggleState();
        }
        
        private void OnChangeInspector()
        {
            this.m_BtnToggleInspector.Q<Image>().image = this.CombosTool.Inspector.State
                ? ICON_INSPECTOR_ON.Texture
                : ICON_INSPECTOR_OFF.Texture;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Button CreateButton(Action callback, IIcon icon, string tip, params string[] classes)
        {
            Button button = new Button(callback) { tooltip = tip };
            Image image = new Image { image = icon?.Texture };
            
            button.Add(image);
            foreach (string className in classes)
            {
                button.AddToClassList(className);   
            }

            return button;
        }
        
        private VisualElement CreateSeparator()
        {
            return new VisualElement { name = NAME_SEPARATOR };
        }

        private VisualElement CreateSpace()
        {
            return new VisualElement { name = NAME_SPACE };
        }
    }
}