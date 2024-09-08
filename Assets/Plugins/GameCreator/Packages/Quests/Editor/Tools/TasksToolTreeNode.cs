using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class TasksToolTreeNode : VisualElement
    {
        private static readonly IIcon ICON_TASK = new IconTaskOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_SUBTASK_SEQ = new IconSubtaskSequence(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_SUBTASK_NOR = new IconSubtaskCombination(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_SUBTASK_ONE = new IconSubtaskSingle(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_SUBTASK_MAN = new IconSubtaskManual(ColorTheme.Type.TextLight);

        private const string NAME_ICON = "GC-Quests-Node-Icon";
        private const string NAME_TEXT = "GC-Quests-Node-Task";

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly Image m_Icon;
        private readonly Label m_Text;

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty PropertyData { get; set; }
        private int Id { get; set; }
        
        private TasksTool TasksTool { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TasksToolTreeNode(TasksTool tasksTool)
        {
            this.TasksTool = tasksTool;

            this.m_Icon = new Image
            {
                name = NAME_ICON,
                image = ICON_TASK.Texture
            };

            this.m_Text = new Label
            {
                name = NAME_TEXT,
                text = string.Empty
            };

            this.Add(this.m_Icon);
            this.Add(this.m_Text);

            ContextualMenuManipulator man = new ContextualMenuManipulator(this.OnOpenMenu);
            this.AddManipulator(man);
            
            this.RegisterCallback<MouseDownEvent>(mouseEvent =>
            {
                if (mouseEvent.clickCount != 2) return;
                this.TasksTool.Inspector.ToggleState();
            });
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void BindItem(SerializedProperty propertyData, int id)
        {
            this.PropertyData = propertyData;
            this.Id = id;
            
            this.Refresh();
            
            this.TasksTool.Inspector.EventChange -= this.Refresh;
            this.TasksTool.Inspector.EventChange += this.Refresh;
        }

        public void UnbindItem()
        {
            this.TasksTool.Inspector.EventChange -= this.Refresh;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Refresh()
        {
            Task task = this.PropertyData.GetValue<Task>();
            if (task == null) return;
            
            string text = task.ToString();
            this.m_Text.text = text;
            this.m_Text.style.display = !string.IsNullOrEmpty(text)
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            int parentId = this.TasksTool.Instance.Parent(this.Id);
            if (parentId == TreeNode.INVALID)
            {
                this.m_Icon.image = ICON_SUBTASK_SEQ.Texture;
            }
            else
            {
                TaskType parentType = this.TasksTool.Instance.Get(parentId).Completion;
                this.m_Icon.image = parentType switch
                {
                    TaskType.SubtasksInSequence => ICON_SUBTASK_SEQ.Texture,
                    TaskType.SubtasksInCombination => ICON_SUBTASK_NOR.Texture,
                    TaskType.AnySubtask => ICON_SUBTASK_ONE.Texture,
                    TaskType.Manual => ICON_SUBTASK_MAN.Texture,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        private void OnOpenMenu(ContextualMenuPopulateEvent menu)
        {
            menu.menu.AppendAction(
                "Delete",
                _ =>
                {
                    Task task = this.PropertyData.GetValue<Task>();
                    if (task == null) return;

                    bool valid = this.TasksTool.Tree.Select(this.Id);
                    if (!valid) return;

                    this.TasksTool.Tree.RemoveSelection();
                });
        }
    }
}