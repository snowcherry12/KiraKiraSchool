using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class TasksToolTree : VisualElement
    {
        public const int DEFAULT_HEIGHT = 30;
        public const int DEFAULT_MAX_ROWS = 20;
        
        private const string KEY_MAX_ROWS = "gc:quests:max-rows";
        
        private const string TITLE_DELETE = "Are you sure you want to delete this element?";
        private const string MSG_DELETE = "This action cannot be undone";

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly TreeView m_TreeView;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public TasksTool TasksTool { get; }
        
        public int Rows
        {
            get => EditorPrefs.GetInt(KEY_MAX_ROWS, DEFAULT_MAX_ROWS);
            set
            {
                int max = Math.Max(value, 10);
                EditorPrefs.SetInt(KEY_MAX_ROWS, max);
                this.RefreshMaxHeight();
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int> EventSelection;
        public event Action EventChange;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TasksToolTree(TasksTool tasksTool)
        {
            this.TasksTool = tasksTool;

            TasksTree tasksTree = this.TasksTool.Instance;
            int[] roots = tasksTree.RootIds;
            
            List<TreeViewItemData<Task>> entries = new List<TreeViewItemData<Task>>();
            foreach (int rootId in roots) entries.AddRange(this.GetTree(tasksTree, rootId));

            this.m_TreeView = new TreeView
            {
                fixedItemHeight = DEFAULT_HEIGHT,
                horizontalScrollingEnabled = true,
                makeItem = this.MakeItem,
                bindItem = this.OnBindItem,
                unbindItem = this.OnUnbindItem,
            };
            
            this.m_TreeView.SetRootItems(entries);
            
            this.m_TreeView.reorderable = true;
            this.m_TreeView.selectionType = SelectionType.Single;
            this.m_TreeView.showAlternatingRowBackgrounds = AlternatingRowBackground.All;

            this.m_TreeView.itemIndexChanged += this.ReorderItems;
            
            // TODO: [21/03/2023] Remove once Unity 2022.3 LTS is released and widely supported
            
            #if UNITY_2022_2_OR_NEWER
            this.m_TreeView.selectionChanged += this.SelectionChange;
            #else
            this.m_TreeView.onSelectionChange += this.SelectionChange;
            #endif
            
            this.m_TreeView.RegisterCallback<KeyDownEvent>(keyEvent =>
            {
                if (this.m_TreeView.selectedIndex < 0) return;
                if (keyEvent.keyCode != KeyCode.Delete && keyEvent.keyCode != KeyCode.Backspace)
                {
                    return;
                }
                
                this.RemoveSelection();
            });
            
            this.Add(this.m_TreeView);
            this.RefreshMaxHeight();
        }
        
        public void Setup()
        {
            
        }

        private void SelectionChange(IEnumerable<object> selection)
        {
            int id = this.m_TreeView.selectedIndex >= 0
                ? this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex)
                : TasksTree.NODE_INVALID;
            
            this.EventSelection?.Invoke(id);
        }

        // CALLBACKS: -----------------------------------------------------------------------------
        
        private VisualElement MakeItem() => new TasksToolTreeNode(this.TasksTool);

        private void OnBindItem(VisualElement element, int index)
        {
            int id = this.m_TreeView.GetIdForIndex(index);
            SerializedProperty propertyData = this.TasksTool
                .FindPropertyForId(id)
                .FindPropertyRelative(TTreeDataItem<Task>.NAME_VALUE);
            
            if (element is TasksToolTreeNode tasksToolNode)
            {
                tasksToolNode.BindItem(propertyData, id);
            }
        }

        private void OnUnbindItem(VisualElement element, int index)
        {
            if (element is TasksToolTreeNode nodeTool)
            {
                nodeTool.UnbindItem();
            }
        }
        
        private void ReorderItems(int indexSource, int indexTarget)
        {
            this.m_TreeView.viewController.RebuildTree();
            this.m_TreeView.RefreshItems();
            
            // int idSource = this.m_TreeView.GetIdForIndex(indexSource);
            // int idTarget = this.m_TreeView.GetIdForIndex(indexSource);
            //
            // this.m_TreeView.ExpandItem(idSource, true);
            // this.m_TreeView.ExpandItem(idTarget, true);
            this.SynchronizeTree();

            this.EventChange?.Invoke();
        }
        
        
        // SETTER METHODS: ------------------------------------------------------------------------

        public void CreateAsSelectionChild(object value)
        {
            if (value is not Task valueTask) return;
            
            if (this.m_TreeView.selectedIndex == -1)
            {
                this.CreateAsSelectionSibling(value);
                return;
            }

            int parentId = this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex);
            
            TasksTree tasksTree = this.TasksTool.Instance;
            int newId = tasksTree.AddChild(valueTask, parentId);

            this.TasksTool.Instance = tasksTree;

            TreeViewItemData<Task> itemData = new TreeViewItemData<Task>(newId, valueTask);
            this.m_TreeView.AddItem(itemData, parentId);
            this.m_TreeView.SetSelectionById(newId);
            
            this.EventChange?.Invoke();
        }
        
        public void CreateAsSelectionSibling(object value)
        {
            if (value is not Task valueTask) return;

            TasksTree tasksTree = this.TasksTool.Instance;
            int[] rootIds = this.m_TreeView.GetRootIds()?.ToArray() ?? Array.Empty<int>();
            
            int selectedId = this.m_TreeView.selectedIndex switch
            {
                -1 => rootIds.Length > 0 ? rootIds[^1] : TasksTree.NODE_INVALID,
                _ => this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex)
            };
            
            int newId = selectedId != TasksTree.NODE_INVALID
                ? tasksTree.AddAfterSibling(valueTask, selectedId)
                : tasksTree.AddToRoot(valueTask);

            this.TasksTool.Instance = tasksTree;

            int parentId = this.m_TreeView.GetParentIdForIndex(this.m_TreeView.selectedIndex);
            int selectedIndex = this.m_TreeView.viewController.GetChildIndexForId(selectedId);

            TreeViewItemData<Task> itemData = new TreeViewItemData<Task>(newId, valueTask);
            this.m_TreeView.AddItem(itemData, parentId, selectedIndex + 1);
            
            this.m_TreeView.SetSelectionById(newId);
            
            this.EventChange?.Invoke();
        }

        public void RemoveSelection(bool confirmationDialog = true)
        {
            if (confirmationDialog)
            {
                bool delete = EditorUtility.DisplayDialog(
                    TITLE_DELETE, MSG_DELETE, 
                    "Yes", "Cancel"
                );
                
                if (!delete) return;
            }

            if (this.m_TreeView.selectedIndex == -1) return;
            int selectedIndex = this.m_TreeView.selectedIndex;

            int selectedId = this.m_TreeView.GetIdForIndex(selectedIndex);
            this.m_TreeView.ExpandItem(selectedId, true);
            
            TasksTree tasksTree = this.TasksTool.Instance;
            
            bool success = tasksTree.Remove(selectedId);
            this.TasksTool.Instance = tasksTree;
            
            if (!success) return;
            
            this.m_TreeView.TryRemoveItem(selectedId);
            
            this.EventChange?.Invoke();
            this.m_TreeView.SetSelection(TasksTree.NODE_INVALID);
        }

        public bool Select(int id)
        {
            int index = this.m_TreeView.viewController.GetIndexForId(id);
            if (index == -1) return false;
            
            this.m_TreeView.SetSelection(index);
            return true;
        }

        // SYNCHRONIZE METHODS: -------------------------------------------------------------------

        private void SynchronizeTree()
        {
            this.m_TreeView.ExpandAll();
            
            SerializationUtils.ApplyUnregisteredSerialization(this.TasksTool.SerializedObject);
            TasksTree tasksTree = this.TasksTool.Instance;

            this.SynchronizeRoots(tasksTree);
            this.SynchronizeNodes(tasksTree);
            
            this.TasksTool.Instance = tasksTree;
        }
        
        private void SynchronizeRoots(TasksTree tasksTree)
        {
            tasksTree.RootIds = this.m_TreeView.GetRootIds()?.ToArray() ?? Array.Empty<int>();
        }

        private void SynchronizeNodes(TasksTree tasksTree)
        {
            int[] rootIds = this.m_TreeView.GetRootIds()?.ToArray() ?? Array.Empty<int>();

            foreach (int rootId in rootIds)
            {
                SynchronizeNode(tasksTree, rootId, TasksTree.NODE_INVALID);
            }
        }

        private void SynchronizeNode(TasksTree tasksTree, int node, int ancestor)
        {
            int index = this.m_TreeView.viewController.GetIndexForId(node);
            int[] childrenIds = this.m_TreeView.GetChildrenIdsForIndex(index)?.ToArray() ?? Array.Empty<int>();

            TreeNode value = tasksTree.Nodes[node]; 
            value.Children = new List<int>(childrenIds);
            value.Parent = ancestor;
            
            foreach (int childId in childrenIds)
            {
                SynchronizeNode(tasksTree, childId, node);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IEnumerable<TreeViewItemData<Task>> GetTree(TasksTree tasksTree, int nodeId)
        {
            Task task = tasksTree.Get(nodeId);
            if (task == null) return new List<TreeViewItemData<Task>>();
        
            List<int> children = tasksTree.Children(nodeId);
            List<TreeViewItemData<Task>> subTree = new List<TreeViewItemData<Task>>();
        
            foreach (int childId in children)
            {
                IEnumerable<TreeViewItemData<Task>> childEntries = this.GetTree(tasksTree, childId);
                subTree.AddRange(childEntries);
            }
        
            return new List<TreeViewItemData<Task>>
            {
                new TreeViewItemData<Task>(nodeId, task, subTree)
            };
        }
        
        private void RefreshMaxHeight()
        {
            if (this.m_TreeView == null) return;

            float height = this.m_TreeView.fixedItemHeight;
            Length length = new Length(height * this.Rows, LengthUnit.Pixel);
            
            this.m_TreeView.style.maxHeight = new StyleLength(length);
        }
        
        // DEBUG METHODS: -------------------------------------------------------------------------
        
        private void DebugPrintTree()
        {
            int[] rootsIds =  this.m_TreeView.GetRootIds().ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            
            this.ToStringTree(stringBuilder, rootsIds, 0);
            Debug.Log(stringBuilder.ToString());
        }
        
        private void ToStringTree(StringBuilder sb, IEnumerable<int> children, int depth)
        {
            if (children == null) return;
            
            foreach (int child in children)
            {
                string text = child.ToString();
                int padding = depth * 4;
                sb.AppendLine(text.PadLeft(text.Length + padding));

                int i = this.m_TreeView.viewController.GetIndexForId(child);
                ToStringTree(sb, this.m_TreeView.GetChildrenIdsForIndex(i)?.ToArray(), depth + 1);
            }
        }
    }
}