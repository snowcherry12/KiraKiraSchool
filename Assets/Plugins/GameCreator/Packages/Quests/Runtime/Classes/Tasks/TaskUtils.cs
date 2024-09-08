using System;
using System.Collections.Generic;

namespace GameCreator.Runtime.Quests
{
    internal static class TaskUtils
    {
        // GETTERS: -------------------------------------------------------------------------------
        
        public static bool CanComplete(Journal journal, Quest quest, int taskId)
        {
            if (quest == null) return false;

            return GetParentTaskType(quest, taskId) switch
            {
                TaskType.SubtasksInSequence => CanCompleteFromSequence(journal, quest, taskId),
                TaskType.SubtasksInCombination => CanCompleteFromCombination(journal, quest, taskId),
                TaskType.AnySubtask => CanCompleteFromSingle(journal, quest, taskId),
                TaskType.Manual => CanCompleteFromManual(journal, quest, taskId),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static bool CanAbandon(Journal journal, Quest quest, int taskId)
        {
            if (quest == null) return false;

            return GetParentTaskType(quest, taskId) switch
            {
                TaskType.SubtasksInSequence => CanAbandonFromSequence(journal, quest, taskId),
                TaskType.SubtasksInCombination => CanAbandonFromCombination(journal, quest, taskId),
                TaskType.AnySubtask => CanAbandonFromSingle(journal, quest, taskId),
                TaskType.Manual => CanAbandonFromManual(journal, quest, taskId),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static bool CanFail(Journal journal, Quest quest, int taskId)
        {
            if (quest == null) return false;

            return GetParentTaskType(quest, taskId) switch
            {
                TaskType.SubtasksInSequence => CanFailFromSequence(journal, quest, taskId),
                TaskType.SubtasksInCombination => CanFailFromCombination(journal, quest, taskId),
                TaskType.AnySubtask => CanFailFromSingle(journal, quest, taskId),
                TaskType.Manual => CanFailFromManual(journal, quest, taskId),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static TaskType GetParentTaskType(Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID
                ? TaskType.SubtasksInSequence
                : quest.GetTask(parentId).Completion;
        }
        
        // SEQUENCE METHODS: ----------------------------------------------------------------------

        private static bool CanCompleteFromSequence(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            if (parentId != TasksTree.NODE_INVALID && !journal.IsTaskActive(quest, parentId))
            {
                return false;
            }

            List<int> siblingIds = quest.Tasks.Siblings(taskId);
            foreach (int siblingId in siblingIds)
            {
                if (siblingId == taskId) return true;
                
                if (journal.IsTaskCompleted(quest, siblingId)) continue;
                return false;
            }

            return quest.GetTask(taskId).Completion switch
            {
                TaskType.SubtasksInSequence => IsEveryChildComplete(journal, quest, taskId),
                TaskType.SubtasksInCombination => IsEveryChildComplete(journal, quest, taskId),
                TaskType.AnySubtask => IsAnyChildComplete(journal, quest, taskId),
                TaskType.Manual => true,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static bool CanAbandonFromSequence(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        private static bool CanFailFromSequence(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        // COMBINATION METHODS: -------------------------------------------------------------------

        private static bool CanCompleteFromCombination(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            if (parentId != TasksTree.NODE_INVALID && !journal.IsTaskActive(quest, parentId))
            {
                return false;
            }

            return quest.GetTask(taskId).Completion switch
            {
                TaskType.SubtasksInSequence => IsEveryChildComplete(journal, quest, taskId),
                TaskType.SubtasksInCombination => IsEveryChildComplete(journal, quest, taskId),
                TaskType.AnySubtask => IsAnyChildComplete(journal, quest, taskId),
                TaskType.Manual => true,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private static bool CanAbandonFromCombination(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        private static bool CanFailFromCombination(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        // SINGLE METHODS: -------------------------------------------------------------------

        private static bool CanCompleteFromSingle(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            if (parentId != TasksTree.NODE_INVALID && !journal.IsTaskActive(quest, parentId))
            {
                return false;
            }
            
            List<int> siblingIds = quest.Tasks.Siblings(taskId);
            foreach (int siblingId in siblingIds)
            {
                if (siblingId == taskId) continue;
                if (journal.IsTaskCompleted(quest, siblingId)) return false;
            }

            return quest.GetTask(taskId).Completion switch
            {
                TaskType.SubtasksInSequence => IsEveryChildComplete(journal, quest, taskId),
                TaskType.SubtasksInCombination => IsEveryChildComplete(journal, quest, taskId),
                TaskType.AnySubtask => IsAnyChildComplete(journal, quest, taskId),
                TaskType.Manual => true,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private static bool CanAbandonFromSingle(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        private static bool CanFailFromSingle(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        // MANUAL METHODS: -------------------------------------------------------------------

        private static bool CanCompleteFromManual(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        private static bool CanAbandonFromManual(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        private static bool CanFailFromManual(Journal journal, Quest quest, int taskId)
        {
            if (!journal.IsTaskActive(quest, taskId)) return false;
            
            int parentId = quest.Tasks.Parent(taskId);
            return parentId == TasksTree.NODE_INVALID || journal.IsTaskActive(quest, parentId);
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        // SEQUENCE METHODS: ----------------------------------------------------------------------
        
        public static async System.Threading.Tasks.Task OnCompleteFromSequence(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            List<int> siblingIds = parentId == TasksTree.NODE_INVALID
                ? new List<int>(quest.Tasks.RootIds)
                : quest.Tasks.Children(parentId);
            
            int siblingIdsCount = siblingIds.Count;
            for (int i = 0; i < siblingIdsCount; i++)
            {
                int siblingId = siblingIds[i];
                if (journal.IsTaskCompleted(quest, siblingId)) continue;
                
                await journal.ActivateTaskWithoutNotify(quest, siblingId);
                return;
            }

            await journal.CompleteTaskWithoutNotify(quest, parentId);
        }
        
        public static async System.Threading.Tasks.Task OnAbandonFromSequence(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (parentId != TasksTree.NODE_INVALID && !journal.IsTaskActive(quest, parentId)) return;
            
            await journal.AbandonTaskWithoutNotify(quest, parentId);
        }
        
        public static async System.Threading.Tasks.Task OnFailFromSequence(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;

            await journal.FailTaskWithoutNotify(quest, parentId);
        }
        
        // COMBINATION METHODS: -------------------------------------------------------------------
        
        public static async System.Threading.Tasks.Task OnCompleteFromCombination(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;
            
            List<int> subtaskIds = quest.Tasks.Children(parentId);
            int subtaskIdsCount = subtaskIds.Count;
            
            for (int i = 0; i < subtaskIdsCount; i++)
            {
                int subtaskId = subtaskIds[i];
                if (!journal.IsTaskCompleted(quest, subtaskId)) return;
            }
            
            await journal.CompleteTaskWithoutNotify(quest, parentId);
        }
        
        public static async System.Threading.Tasks.Task OnAbandonFromCombination(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;

            await journal.AbandonTaskWithoutNotify(quest, parentId);
        }
        
        public static async System.Threading.Tasks.Task OnFailFromCombination(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;

            await journal.FailTaskWithoutNotify(quest, parentId);
        }
        
        // SINGLE METHODS: ------------------------------------------------------------------------
        
        public static async System.Threading.Tasks.Task OnCompleteFromSingle(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;
            
            await journal.CompleteTaskWithoutNotify(quest, parentId);
        }
        
        public static async System.Threading.Tasks.Task OnAbandonFromSingle(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;
            
            List<int> subtaskIds = quest.Tasks.Children(parentId);
            int subtaskIdsCount = subtaskIds.Count;
            
            for (int i = 0; i < subtaskIdsCount; i++)
            {
                int subtaskId = subtaskIds[i];
                if (journal.IsTaskActive(quest, subtaskId)) return;
            }

            await journal.AbandonTaskWithoutNotify(quest, parentId);
        }
        
        public static async System.Threading.Tasks.Task OnFailFromSingle(Journal journal, Quest quest, int taskId)
        {
            int parentId = quest.Tasks.Parent(taskId);
            if (!journal.IsTaskActive(quest, parentId)) return;
            
            List<int> subtaskIds = quest.Tasks.Children(parentId);
            int subtaskIdsCount = subtaskIds.Count;
            
            for (int i = 0; i < subtaskIdsCount; i++)
            {
                int subtaskId = subtaskIds[i];
                if (journal.IsTaskActive(quest, subtaskId)) return;
            }

            await journal.FailTaskWithoutNotify(quest, parentId);
        }
        
        // MANUAL METHODS: ------------------------------------------------------------------------
        
        public static System.Threading.Tasks.Task OnCompleteFromManual(Journal journal, Quest quest, int taskId)
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }
        
        public static System.Threading.Tasks.Task OnAbandonFromManual(Journal journal, Quest quest, int taskId)
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }
        
        public static System.Threading.Tasks.Task OnFailFromManual(Journal journal, Quest quest, int taskId)
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static bool IsEveryChildComplete(Journal journal, Quest quest, int taskId)
        {
            List<int> subtaskIds = quest.Tasks.Children(taskId);
            foreach (int subtaskId in subtaskIds)
            {
                if (journal.IsTaskCompleted(quest, subtaskId)) continue;
                return false;
            }

            return true;
        }
        
        private static bool IsAnyChildComplete(Journal journal, Quest quest, int taskId)
        {
            List<int> subtaskIds = quest.Tasks.Children(taskId);
            foreach (int subtaskId in subtaskIds)
            {
                if (journal.IsTaskCompleted(quest, subtaskId))
                {
                    return true;
                }
            }

            return true;
        }
    }
}