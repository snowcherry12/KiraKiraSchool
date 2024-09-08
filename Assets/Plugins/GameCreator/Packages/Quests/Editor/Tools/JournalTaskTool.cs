using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class JournalTaskTool : VisualElement
    {
        private static readonly IIcon ICON_EXPAND_NONE = new IconDot(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_EXPAND_ON = new IconChevronDown(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_EXPAND_OFF = new IconChevronRight(ColorTheme.Type.TextLight);

        private static readonly IIcon ICON_TASK_INACTIVE = new IconTaskOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_TASK_ACTIVE = new IconTaskOutline(ColorTheme.Type.Blue);
        private static readonly IIcon ICON_TASK_COMPLETED = new IconTaskSolid(ColorTheme.Type.Green);
        private static readonly IIcon ICON_TASK_ABANDONED = new IconTaskSolid(ColorTheme.Type.Yellow);
        private static readonly IIcon ICON_TASK_FAILED = new IconTaskSolid(ColorTheme.Type.Red);

        private const string NAME_HEAD = "GC-Quests-Journal-Task-Head";
        private const string NAME_BODY = "GC-Quests-Journal-Task-Body";

        private const int INDENT_SIZE = 10;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly VisualElement m_Head;
        private readonly VisualElement m_Body;
        
        private readonly Image m_ExpandIcon;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public JournalTaskTool(Journal journal, Quest quest, int taskId, int indent)
        {
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new VisualElement { name = NAME_BODY };

            this.m_ExpandIcon = new Image
            {
                image = SessionState.GetBool(GetTaskKey(taskId), true)
                    ? ICON_EXPAND_ON.Texture
                    : ICON_EXPAND_OFF.Texture
            };
            
            this.m_Head.Add(this.m_ExpandIcon);

            State taskState = journal.GetTaskState(quest, taskId);            
            this.m_Head.Add(new Image
            {
                image = taskState switch
                {
                    State.Inactive => ICON_TASK_INACTIVE.Texture,
                    State.Active => ICON_TASK_ACTIVE.Texture,
                    State.Completed => ICON_TASK_COMPLETED.Texture,
                    State.Abandoned => ICON_TASK_ABANDONED.Texture,
                    State.Failed => ICON_TASK_FAILED.Texture,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });

            Task task = quest.GetTask(taskId);
            string title = TextUtils.Humanize(task);

            if (task.UseCounter != ProgressType.None)
            {
                title = $"{title} (Counter: {task.MaximumValueString})";
            }
            
            this.m_Head.Add(new Label(title));
            this.m_Head.Add(new FlexibleSpace());
            this.m_Head.Add(new Label(TextUtils.Humanize(taskState)));
            
            List<int> childrenTaskIds = quest.Tasks.Children(taskId);
            foreach (int childTaskId in childrenTaskIds)
            {
                JournalTaskTool subTask = new JournalTaskTool(
                    journal, quest, childTaskId, 
                    indent + 1
                );
                
                this.m_Body.Add(subTask);
            }
            
            this.Add(this.m_Head);
            this.Add(this.m_Body);

            if (childrenTaskIds.Count > 0)
            {
                this.m_Head.RegisterCallback<MouseDownEvent>(_ =>
                {
                    bool newState = !SessionState.GetBool(GetTaskKey(taskId), true);
                    SessionState.SetBool(GetTaskKey(taskId), newState);

                    this.m_ExpandIcon.image = newState
                        ? ICON_EXPAND_ON.Texture
                        : ICON_EXPAND_OFF.Texture;

                    this.m_Body.style.display = newState 
                        ? DisplayStyle.Flex 
                        : DisplayStyle.None;
                });
            }
            else
            {
                this.m_ExpandIcon.image = ICON_EXPAND_NONE.Texture;
            }

            this.m_Body.style.display = SessionState.GetBool(GetTaskKey(taskId), true) 
                ? DisplayStyle.Flex 
                : DisplayStyle.None;
            this.style.marginLeft = new Length(indent * INDENT_SIZE, LengthUnit.Pixel);
        }
        
        // STATIC METHODS: ------------------------------------------------------------------------

        private static string GetTaskKey(int taskId)
        {
            return $"gc:quests:expand-journal-task-{taskId}";
        }
    }
}