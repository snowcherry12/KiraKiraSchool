using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    internal class Tasks
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private TaskEntries m_Tasks = new TaskEntries();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Journal m_Journal;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public TaskEntries Entries
        {
            get => this.m_Tasks;
            set => this.m_Tasks = value;
        }
        
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action<Quest, int> EventActivate;
        public event Action<Quest, int> EventDeactivate;
        
        public event Action<Quest, int> EventComplete;
        public event Action<Quest, int> EventAbandon;
        public event Action<Quest, int> EventFail;
        
        public event Action<Quest, int> EventValueChange;
        
        // INITIALIZE METHODS: --------------------------------------------------------------------

        public void Setup(Journal journal)
        {
            this.m_Journal = journal;
        }
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public State GetState(Quest quest, int taskId)
        {
            return this.m_Tasks.GetState(quest, taskId);
        }
        
        public bool IsActive(Quest quest, int taskId)
        {
            return this.m_Tasks.IsActive(quest, taskId);
        }
        
        public bool IsInactive(Quest quest, int taskId)
        {
            return this.m_Tasks.IsInactive(quest, taskId);
        }
        
        public bool IsCompleted(Quest quest, int taskId)
        {
            return this.m_Tasks.IsCompleted(quest, taskId);
        }
        
        public bool IsAbandoned(Quest quest, int taskId)
        {
            return this.m_Tasks.IsAbandoned(quest, taskId);
        }
        
        public bool IsFailed(Quest quest, int taskId)
        {
            return this.m_Tasks.IsFailed(quest, taskId);
        }

        // SETTER METHODS: ------------------------------------------------------------------------
        
        public async System.Threading.Tasks.Task<bool> Activate(Quest quest, int taskId)
        {
            if (quest == null) return false;
            double value = 0;

            TaskKey taskKey = new TaskKey(quest, taskId);
            if (this.m_Tasks.TryGetValue(taskKey, out TaskEntry entry))
            {
                if (!entry.State.IsInactive()) return false;
                value = entry.Value;
            }

            List<int> subtaskIds = quest.Tasks.Children(taskId);
            switch (quest.GetTask(taskId).Completion)
            {
                case TaskType.SubtasksInSequence:
                    if (subtaskIds.Count >= 1) await this.Activate(quest, subtaskIds[0]);
                    break;
                
                case TaskType.SubtasksInCombination:
                    foreach (int subtaskId in subtaskIds) await this.Activate(quest, subtaskId);
                    break;
                
                case TaskType.AnySubtask:
                    foreach (int subtaskId in subtaskIds) await this.Activate(quest, subtaskId);
                    break;
                
                case TaskType.Manual: 
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }

            this.m_Tasks[taskKey] = TaskEntry.NewActive(value);
            Task task = quest.GetTask(taskId);
            
            this.m_Journal.OnEnableTask(quest, taskId);
            await task.RunOnActivate(this.m_Journal.Args);

            this.EventActivate?.Invoke(quest, taskId);

            if (task.UseCounter == ProgressType.None) return true;
            if (this.m_Tasks[taskKey].Value >= task.GetCountTo(this.m_Journal.Args))
            {
                await this.Complete(quest, taskId);
            }
            
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Deactivate(Quest quest, int taskId)
        {
            if (quest == null) return false;

            TaskKey taskKey = new TaskKey(quest, taskId);
            if (this.m_Tasks.TryGetValue(taskKey, out TaskEntry entry))
            {
                if (entry.State.IsInactive()) return false;
            }
            
            List<int> subtaskIds = quest.Tasks.Children(taskId);
            foreach (int subtaskId in subtaskIds)
            {
                await this.Deactivate(quest, subtaskId);
            }

            this.m_Tasks[taskKey] = TaskEntry.NewInactive(0);
            Task task = quest.GetTask(taskId);
            
            await task.RunOnDeactivate(this.m_Journal.Args);
            this.m_Journal.OnDisableTask(quest, taskId);

            this.EventDeactivate?.Invoke(quest, taskId);
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Complete(Quest quest, int taskId)
        {
            if (!await this.RunComplete(quest, taskId)) return false;
            
            this.m_Journal.OnDisableTask(quest, taskId);
            this.EventComplete?.Invoke(quest, taskId);

            await this.EvaluateQuestOnChangeTask(quest);
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Abandon(Quest quest, int taskId)
        {
            if (!await this.RunAbandon(quest, taskId)) return false;
            
            this.m_Journal.OnDisableTask(quest, taskId);
            this.EventAbandon?.Invoke(quest, taskId);
            
            await this.EvaluateQuestOnChangeTask(quest);
            return true;
        }
        
        public async System.Threading.Tasks.Task<bool> Fail(Quest quest, int taskId)
        {
            if (!await this.RunFail(quest, taskId)) return false;
            
            this.m_Journal.OnDisableTask(quest, taskId);
            this.EventFail?.Invoke(quest, taskId);
            
            await this.EvaluateQuestOnChangeTask(quest);
            return true;
        }

        private async System.Threading.Tasks.Task<bool> RunComplete(Quest quest, int taskId)
        {
            bool canComplete = TaskUtils.CanComplete(this.m_Journal, quest, taskId); 
            if (!canComplete) return false;
            
            double value = 0;
            TaskKey taskKey = new TaskKey(quest, taskId);
            
            if (this.m_Tasks.TryGetValue(taskKey, out TaskEntry entry)) value = entry.Value;
            this.m_Tasks[taskKey] = TaskEntry.NewCompleted(value);
            
            await quest.GetTask(taskId).RunOnComplete(this.m_Journal.Args);
            await this.OnComplete(quest, taskId);

            return true;
        }

        private async System.Threading.Tasks.Task<bool> RunAbandon(Quest quest, int taskId)
        {
            bool canAbandon = TaskUtils.CanAbandon(this.m_Journal, quest, taskId); 
            if (!canAbandon) return false;
            
            double value = 0;
            TaskKey taskKey = new TaskKey(quest, taskId);
            
            if (this.m_Tasks.TryGetValue(taskKey, out TaskEntry entry)) value = entry.Value;
            this.m_Tasks[taskKey] = TaskEntry.NewAbandoned(value);
            
            await quest.GetTask(taskId).RunOnAbandon(this.m_Journal.Args);
            await this.OnAbandon(quest, taskId);

            return true;
        }
        
        private async System.Threading.Tasks.Task<bool> RunFail(Quest quest, int taskId)
        {
            bool canFail = TaskUtils.CanFail(this.m_Journal, quest, taskId); 
            if (!canFail) return false;
            
            double value = 0;
            TaskKey taskKey = new TaskKey(quest, taskId);
            
            if (this.m_Tasks.TryGetValue(taskKey, out TaskEntry entry)) value = entry.Value;
            this.m_Tasks[taskKey] = TaskEntry.NewFailed(value);
            
            await quest.GetTask(taskId).RunOnFail(this.m_Journal.Args);
            await this.OnFail(quest, taskId);

            return true;
        }

        // PROGRESS METHODS: ----------------------------------------------------------------------

        public double GetValue(Quest quest, int taskId)
        {
            TaskKey taskKey = new TaskKey(quest, taskId);
            if (quest != null && this.m_Tasks.TryGetValue(taskKey, out TaskEntry taskEntry))
            {
                Task task = quest.GetTask(taskId);
                return task.UseCounter switch
                {
                    ProgressType.None => 0,
                    ProgressType.Value => taskEntry.Value,
                    ProgressType.Property => taskEntry.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            
            return 0;
        }
        
        public bool SetValue(Quest quest, int taskId, double value)
        {
            if (!this.m_Tasks.IsActive(quest, taskId)) return false;
            
            TaskKey taskKey = new TaskKey(quest, taskId);
            this.m_Tasks.TryGetValue(taskKey, out TaskEntry task);
            
            task.Value = value;
            this.m_Tasks[taskKey] = task;

            this.EventValueChange?.Invoke(quest, taskId);
            return true;
        }
        
        // PRIVATE UTIL METHODS: ------------------------------------------------------------------

        private async System.Threading.Tasks.Task OnComplete(Quest quest, int taskId)
        {
            await (TaskUtils.GetParentTaskType(quest, taskId) switch
            {
                TaskType.SubtasksInSequence => TaskUtils.OnCompleteFromSequence(this.m_Journal, quest, taskId),
                TaskType.SubtasksInCombination => TaskUtils.OnCompleteFromCombination(this.m_Journal, quest, taskId),
                TaskType.AnySubtask => TaskUtils.OnCompleteFromSingle(this.m_Journal, quest, taskId),
                TaskType.Manual => TaskUtils.OnCompleteFromManual(this.m_Journal, quest, taskId),
                _ => throw new ArgumentOutOfRangeException()
            });
        }
        
        private async System.Threading.Tasks.Task OnAbandon(Quest quest, int taskId)
        {
            await (TaskUtils.GetParentTaskType(quest, taskId) switch
            {
                TaskType.SubtasksInSequence => TaskUtils.OnAbandonFromSequence(this.m_Journal, quest, taskId),
                TaskType.SubtasksInCombination => TaskUtils.OnAbandonFromCombination(this.m_Journal, quest, taskId),
                TaskType.AnySubtask => TaskUtils.OnAbandonFromSingle(this.m_Journal, quest, taskId),
                TaskType.Manual => TaskUtils.OnAbandonFromManual(this.m_Journal, quest, taskId),
                _ => throw new ArgumentOutOfRangeException()
            });
        }
        
        private async System.Threading.Tasks.Task OnFail(Quest quest, int taskId)
        {
            await (TaskUtils.GetParentTaskType(quest, taskId) switch
            {
                TaskType.SubtasksInSequence => TaskUtils.OnFailFromSequence(this.m_Journal, quest, taskId),
                TaskType.SubtasksInCombination => TaskUtils.OnFailFromCombination(this.m_Journal, quest, taskId),
                TaskType.AnySubtask => TaskUtils.OnFailFromSingle(this.m_Journal, quest, taskId),
                TaskType.Manual => TaskUtils.OnFailFromManual(this.m_Journal, quest, taskId),
                _ => throw new ArgumentOutOfRangeException()
            });
        }
        
        private async System.Threading.Tasks.Task<bool> EvaluateQuestOnChangeTask(Quest quest)
        {
            int[] rootIds = quest.Tasks.RootIds;
            foreach (int rootId in rootIds)
            {
                TaskKey taskKey = new TaskKey(quest, rootId);
                if (!this.m_Tasks.TryGetValue(taskKey, out TaskEntry entry)) return false;

                switch (entry.State)
                {
                    case State.Inactive: return false;
                    case State.Active: return false;
                    case State.Completed: continue;
                    case State.Abandoned: return await this.m_Journal.AbandonQuestWithoutNotify(quest);
                    case State.Failed: return await this.m_Journal.FailQuestWithoutNotify(quest);
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        
            return await this.m_Journal.CompleteQuestWithoutNotify(quest);
        }
    }
}