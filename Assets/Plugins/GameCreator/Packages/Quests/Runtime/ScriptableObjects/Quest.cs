using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [CreateAssetMenu(
        fileName = "Quest", 
        menuName = "Game Creator/Quests/Quest"
    )]
    
    [Icon(EditorPaths.PACKAGES + "Quests/Editor/Gizmos/GizmoQuest.png")]
    
    [Serializable]
    public class Quest : ScriptableObject, IEquatable<Quest>
    {
        public static Quest LastQuestActivated;
        public static Quest LastQuestDeactivated;
        public static Quest LastQuestCompleted;
        public static Quest LastQuestAbandoned;
        public static Quest LastQuestFailed;
        
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            LastQuestActivated = null;
            LastQuestDeactivated = null;
            LastQuestCompleted = null;
            LastQuestAbandoned = null;
            LastQuestFailed = null;
        }
        
        #endif

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Title = GetStringString.Create;
        [SerializeField] private PropertyGetString m_Description = GetStringEmpty.Create;

        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;
        [SerializeField] private PropertyGetSprite m_Sprite = GetSpriteNone.Create;
        
        [SerializeField] private QuestType m_Type = QuestType.Normal;
        [SerializeField] private int m_SortOrder;
        
        [SerializeField] private UniqueID m_UniqueId;
        [SerializeReference] private TasksTree m_Tasks = new TasksTree();
        
        [SerializeField] private RunInstructionsList m_OnActivate = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnDeactivate = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnComplete = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnAbandon = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnFail = new RunInstructionsList();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString Id => this.m_UniqueId.Get;

        public QuestType Type => this.m_Type;
        public int SortOrder => this.m_SortOrder;
        
        public TasksTree Tasks => this.m_Tasks;

        // GETTERS: -------------------------------------------------------------------------------

        public string GetTitle(Args args) => this.m_Title.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);
        
        public Color GetColor(Args args) => this.m_Color.Get(args);
        public Sprite GetSprite(Args args) => this.m_Sprite.Get(args);
        
        public Task GetTask(int taskId) => this.m_Tasks.Get(taskId);

        public bool Contains(int taskId) => this.m_Tasks.Contains(taskId);

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public async System.Threading.Tasks.Task RunOnActivate(Args args)
        {
            await this.m_OnActivate.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Activate Quest",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnDeactivate(Args args)
        {
            await this.m_OnDeactivate.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Deactivate Quest",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnComplete(Args args)
        {
            await this.m_OnComplete.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Complete Quest",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnAbandon(Args args)
        {
            await this.m_OnAbandon.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Abandon Quest",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }
        
        public async System.Threading.Tasks.Task RunOnFail(Args args)
        {
            await this.m_OnFail.Run(
                args.Clone, 
                new RunnerConfig
                {
                    Name = "On Fail Quest",
                    Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
                }
            );
        }

        // EQUALITY METHODS: ----------------------------------------------------------------------

        public bool Equals(Quest other)
        {
            return other != null && this.Id.Hash == other.Id.Hash;
        }

        public override bool Equals(object other)
        {
            return other is Quest otherQuest && this.Equals(otherQuest);
        }

        public override int GetHashCode()
        {
            return this.Id.Hash;
        }
    }
}