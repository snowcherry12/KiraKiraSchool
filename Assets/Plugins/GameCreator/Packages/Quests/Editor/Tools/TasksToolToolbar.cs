using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class TasksToolToolbar : VisualElement
    {
        private static readonly IIcon ICON_TASK = new IconTaskSolid(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_SUBTASK = new IconTaskOutline(ColorTheme.Type.TextNormal);
        
        private static readonly IIcon ICON_INSPECTOR_ON = new IconSidebar(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_INSPECTOR_OFF = new IconSidebar(ColorTheme.Type.TextLight);

        private const string TIP_TASK = "Add new Task";
        private const string TIP_SUBTASK = "Add new SubTask";
        private const string TIP_INSPECTOR = "Toggle Inspector";

        private const string NAME_SEPARATOR = "GC-Quests-Toolbar-Separator";
        private const string NAME_SPACE = "GC-Quests-Toolbar-Space";

        private const string CLASS_BUTTON = "gc-quests-toolbar-button";

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Button m_BtnTask;
        [NonSerialized] private readonly Button m_BtnSubTask;
        [NonSerialized] private readonly Button m_BtnToggleInspector;

        [NonSerialized] private readonly DropdownField m_DropdownInsertMode;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private TasksTool TasksTool { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TasksToolToolbar(TasksTool contentTool)
        {
            this.TasksTool = contentTool;
            
            this.m_BtnTask = this.CreateButton(
                this.AddTask, 
                ICON_TASK,
                TIP_TASK, 
                CLASS_BUTTON
            );
            
            this.m_BtnSubTask = this.CreateButton(
                this.AddSubTask, 
                ICON_SUBTASK,
                TIP_SUBTASK, 
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
            this.Add(this.m_BtnTask);
            this.Add(this.CreateSpace());
            this.Add(new Label("or"));
            this.Add(this.CreateSpace());
            this.Add(this.m_BtnSubTask);
            this.Add(new FlexibleSpace());
            this.Add(this.CreateSeparator());
            this.Add(this.m_BtnToggleInspector);
        }

        public void Setup()
        {
            this.TasksTool.Inspector.EventState += this.OnChangeInspector;
            this.OnChangeInspector();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void AddTask()
        {
            Task newNode = new Task();
            this.TasksTool.Tree.CreateAsSelectionSibling(newNode);
        }
        
        private void AddSubTask()
        {
            Task newNode = new Task();
            this.TasksTool.Tree.CreateAsSelectionChild(newNode);
        }

        private void ToggleInspector()
        {
            this.TasksTool.Inspector.ToggleState();
        }
        
        private void OnChangeInspector()
        {
            this.m_BtnToggleInspector.Q<Image>().image = this.TasksTool.Inspector.State
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