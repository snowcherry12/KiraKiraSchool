using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class ContentToolToolbar : VisualElement
    {
        private static readonly IIcon ICON_TEXT = new IconNodeText(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_CHOICE = new IconNodeChoice(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_RANDOM = new IconNodeRandom(ColorTheme.Type.TextNormal);
        
        private static readonly IIcon ICON_SETTINGS_ON = new IconGear(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_SETTINGS_OFF = new IconGear(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_INSPECTOR_ON = new IconSidebar(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_INSPECTOR_OFF = new IconSidebar(ColorTheme.Type.TextLight);

        private const string TIP_TEXT = "Add new Text entry";
        private const string TIP_CHOICE = "Add new Choice group entry";
        private const string TIP_RANDOM = "Add new Random selection group";
        private const string TIP_SETTINGS = "Toggle Settings";
        private const string TIP_INSPECTOR = "Toggle Inspector";

        private const string NAME_SEPARATOR = "GC-Dialogue-Toolbar-Separator";
        private const string NAME_SPACE = "GC-Dialogue-Toolbar-Space";

        private const string CLASS_EDGE_L = "gc-dialogue-toolbar-edge-left";
        private const string CLASS_EDGE_R = "gc-dialogue-toolbar-edge-right";

        private const string KEY_INSERT_INSIDE = "gc:dialogue:settings:insert-mode";
        
        private static readonly List<string> INSERT_MODES_TEXT = new List<string>
        {
            "Sibling",
            "Child",
        };

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Button m_BtnText;
        [NonSerialized] private readonly Button m_BtnChoice;
        [NonSerialized] private readonly Button m_BtnRandom;
        [NonSerialized] private readonly Button m_BtnToggleSettings;
        [NonSerialized] private readonly Button m_BtnToggleInspector;

        [NonSerialized] private readonly DropdownField m_DropdownInsertMode;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private ContentTool ContentTool { get; }

        public bool InsertInside
        {
            get => EditorPrefs.GetBool(KEY_INSERT_INSIDE, true);
            set => EditorPrefs.SetBool(KEY_INSERT_INSIDE, value);
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ContentToolToolbar(ContentTool contentTool)
        {
            this.ContentTool = contentTool;
            
            this.m_BtnText = this.CreateButton(this.AddText, ICON_TEXT, TIP_TEXT, CLASS_EDGE_L);
            this.m_BtnChoice = this.CreateButton(this.AddChoice, ICON_CHOICE, TIP_CHOICE);
            this.m_BtnRandom = this.CreateButton(this.AddRandom, ICON_RANDOM, TIP_RANDOM, CLASS_EDGE_R);
            
            this.m_BtnToggleSettings = this.CreateButton(this.ToggleSettings, ICON_SETTINGS_OFF, TIP_SETTINGS, CLASS_EDGE_L);
            this.m_BtnToggleInspector = this.CreateButton(this.ToggleInspector, ICON_INSPECTOR_OFF, TIP_INSPECTOR, CLASS_EDGE_R);

            this.m_DropdownInsertMode = new DropdownField(
                INSERT_MODES_TEXT,
                this.InsertInside ? 1 : 0,
                choice => choice,
                choice => choice
            );

            this.m_DropdownInsertMode.RegisterValueChangedCallback(changeEvent =>
            {
                this.InsertInside = changeEvent.newValue != INSERT_MODES_TEXT[0];
            });

            this.Add(new Label("Insert"));
            this.Add(this.CreateSpace());
            this.Add(this.m_BtnText);
            this.Add(this.CreateSeparator());
            this.Add(this.m_BtnChoice);
            this.Add(this.CreateSeparator());
            this.Add(this.m_BtnRandom);
            this.Add(this.CreateSpace());
            this.Add(new Label("as"));
            this.Add(this.CreateSpace());
            this.Add(this.m_DropdownInsertMode);
            this.Add(new FlexibleSpace());
            this.Add(this.CreateSeparator());
            this.Add(this.m_BtnToggleSettings);
            this.Add(this.CreateSeparator());
            this.Add(this.m_BtnToggleInspector);
        }

        public void Setup()
        {
            this.ContentTool.Settings.EventState += this.OnChangeSettings;
            this.ContentTool.Inspector.EventState += this.OnChangeInspector;
            
            this.OnChangeSettings();
            this.OnChangeInspector();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void AddText(ClickEvent clickEvent)
        {
            Node newNode = new Node { NodeType = new NodeTypeText() };
            bool insertInside = clickEvent.modifiers == EventModifiers.Shift
                ? !this.InsertInside
                : this.InsertInside;
            
            if (insertInside) this.ContentTool.Tree.CreateAsSelectionChild(newNode);
            else this.ContentTool.Tree.CreateAsSelectionSibling(newNode);
        }
        
        private void AddChoice(ClickEvent clickEvent)
        {
            Node newNode = new Node { NodeType = new NodeTypeChoice() };
            bool insertInside = clickEvent.modifiers == EventModifiers.Shift
                ? !this.InsertInside
                : this.InsertInside;
            
            if (insertInside) this.ContentTool.Tree.CreateAsSelectionChild(newNode);
            else this.ContentTool.Tree.CreateAsSelectionSibling(newNode);
        }
        
        private void AddRandom(ClickEvent clickEvent)
        {
            Node newNode = new Node { NodeType = new NodeTypeRandom() };
            bool insertInside = clickEvent.modifiers == EventModifiers.Shift
                ? !this.InsertInside
                : this.InsertInside;
            
            if (insertInside) this.ContentTool.Tree.CreateAsSelectionChild(newNode);
            else this.ContentTool.Tree.CreateAsSelectionSibling(newNode);
        }
        
        private void ToggleSettings(ClickEvent clickEvent)
        {
            this.ContentTool.Settings.ToggleState();
        }

        private void ToggleInspector(ClickEvent clickEvent)
        {
            this.ContentTool.Inspector.ToggleState();
        }
        
        private void OnChangeInspector()
        {
            this.m_BtnToggleInspector.Q<Image>().image = this.ContentTool.Inspector.State
                ? ICON_INSPECTOR_ON.Texture
                : ICON_INSPECTOR_OFF.Texture;
        }

        private void OnChangeSettings()
        {
            this.m_BtnToggleSettings.Q<Image>().image = this.ContentTool.Settings.State
                ? ICON_SETTINGS_ON.Texture
                : ICON_SETTINGS_OFF.Texture;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Button CreateButton(EventCallback<ClickEvent> callback, IIcon icon, string tip, params string[] classes)
        {
            Button button = new Button { tooltip = tip };
            button.RegisterCallback(callback);
            
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