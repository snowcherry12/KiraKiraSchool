using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class JournalQuestTool : VisualElement
    {
        private static readonly IIcon ICON_EXPAND_ON = new IconChevronDown(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_EXPAND_OFF = new IconChevronRight(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_TRACKING_ON = new IconBookmarkSolid(ColorTheme.Type.Red);
        private static readonly IIcon ICON_TRACKING_OFF = new IconBookmarkOutline(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_QUEST_INACTIVE = new IconQuestSolid(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_QUEST_ACTIVE = new IconQuestSolid(ColorTheme.Type.Blue);
        private static readonly IIcon ICON_QUEST_COMPLETED = new IconQuestSolid(ColorTheme.Type.Green);
        private static readonly IIcon ICON_QUEST_ABANDONED = new IconQuestSolid(ColorTheme.Type.Yellow);
        private static readonly IIcon ICON_QUEST_FAILED = new IconQuestSolid(ColorTheme.Type.Red);

        private const string NAME_HEAD = "GC-Quests-Journal-Quest-Head";
        private const string NAME_BODY = "GC-Quests-Journal-Quest-Body";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly VisualElement m_Head;
        private readonly VisualElement m_Body;
        
        private readonly Image m_ExpandIcon;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public JournalQuestTool(Journal journal, IdString questId, QuestEntry questEntry)
        {
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new VisualElement { name = NAME_BODY };

            Quest quest = null;
            string[] quests = AssetDatabase.FindAssets($"t:{nameof(Quest)}");
            foreach (string questGuid in quests)
            {
                string questPath = AssetDatabase.GUIDToAssetPath(questGuid);
                quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
                
                if (quest == null) continue;
                if (quest.Id.Hash == questId.Hash) break;
            }
            
            if (quest == null) return;

            this.m_ExpandIcon = new Image
            {
                image = SessionState.GetBool(GetQuestKey(quest.Id.Hash), false)
                    ? ICON_EXPAND_ON.Texture
                    : ICON_EXPAND_OFF.Texture
            };
            
            this.m_Head.Add(this.m_ExpandIcon);
            
            this.m_Head.Add(new Image
            {
                image = questEntry.State switch
                {
                    State.Inactive => ICON_QUEST_INACTIVE.Texture,
                    State.Active => ICON_QUEST_ACTIVE.Texture,
                    State.Completed => ICON_QUEST_COMPLETED.Texture,
                    State.Abandoned => ICON_QUEST_ABANDONED.Texture,
                    State.Failed => ICON_QUEST_FAILED.Texture,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });

            this.m_Head.Add(new Label(TextUtils.Humanize(quest.name)));
            this.m_Head.Add(new FlexibleSpace());
            
            this.m_Head.Add(new Image
            {
                image = questEntry.IsTracking 
                    ? ICON_TRACKING_ON.Texture 
                    : ICON_TRACKING_OFF.Texture
            });
            
            this.m_Head.Add(new Label(TextUtils.Humanize(questEntry.State)));

            int[] taskIds = quest.Tasks.RootIds;
            foreach (int taskId in taskIds)
            {
                JournalTaskTool subTask = new JournalTaskTool(journal, quest, taskId, 1);
                this.m_Body.Add(subTask);
            }
            
            this.Add(this.m_Head);
            this.Add(this.m_Body);

            this.m_Head.RegisterCallback<MouseDownEvent>(_ =>
            {
                bool newState = !SessionState.GetBool(GetQuestKey(quest.Id.Hash), false);
                SessionState.SetBool(GetQuestKey(quest.Id.Hash), newState);

                this.m_ExpandIcon.image = newState
                    ? ICON_EXPAND_ON.Texture
                    : ICON_EXPAND_OFF.Texture;

                this.m_Body.style.display = newState 
                    ? DisplayStyle.Flex 
                    : DisplayStyle.None;
            });
            
            this.m_Body.style.display = SessionState.GetBool(GetQuestKey(quest.Id.Hash), false) 
                ? DisplayStyle.Flex 
                : DisplayStyle.None;
        }
        
        // STATIC METHODS: ------------------------------------------------------------------------

        private static string GetQuestKey(int questId)
        {
            return $"gc:quests:expand-journal-quest-{questId}";
        }
    }
}