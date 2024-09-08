using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    public class CombosToolTree : VisualElement
    {
        public const int DEFAULT_HEIGHT = 30;
        public const int DEFAULT_MAX_ROWS = 20;
        
        private const string KEY_MAX_ROWS = "gc:melee:max-rows";
        
        private const string TITLE_DELETE = "Are you sure you want to delete this element?";
        private const string MSG_DELETE = "This action cannot be undone";

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly TreeView m_TreeView;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public CombosTool CombosTool { get; }
        
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
        
        public CombosToolTree(CombosTool combosTool)
        {
            this.CombosTool = combosTool;

            ComboTree comboTree = this.CombosTool.Instance;
            int[] roots = comboTree.RootIds;
            
            List<TreeViewItemData<ComboItem>> entries = new List<TreeViewItemData<ComboItem>>();
            foreach (int rootId in roots) entries.AddRange(this.GetTree(comboTree, rootId));

            this.m_TreeView = new TreeView
            {
                fixedItemHeight = DEFAULT_HEIGHT,
                horizontalScrollingEnabled = true,
                makeItem = this.MakeItem,
                bindItem = this.OnBindItem,
                unbindItem = this.OnUnbindItem
            };
            
            this.m_TreeView.SetRootItems(entries);
            
            this.m_TreeView.reorderable = true;
            this.m_TreeView.selectionType = SelectionType.Single;
            this.m_TreeView.showAlternatingRowBackgrounds = AlternatingRowBackground.All;

            this.m_TreeView.itemIndexChanged += this.ReorderItems;
            this.m_TreeView.selectionChanged += this.SelectionChange;
            
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
            this.EventChange += () =>
            {
                int id = this.m_TreeView.selectedIndex >= 0
                    ? this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex)
                    : ComboTree.NODE_INVALID;

                this.Select(id);
            };
        }

        private void SelectionChange(IEnumerable<object> selection)
        {
            int id = this.m_TreeView.selectedIndex >= 0
                ? this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex)
                : ComboTree.NODE_INVALID;
            
            this.EventSelection?.Invoke(id);
        }

        // CALLBACKS: -----------------------------------------------------------------------------
        
        private VisualElement MakeItem() => new CombosToolTreeNode(this.CombosTool);

        private void OnBindItem(VisualElement element, int index)
        {
            int id = this.m_TreeView.GetIdForIndex(index);
            SerializedProperty propertyData = this.CombosTool
                .FindPropertyForId(id)
                .FindPropertyRelative(TTreeDataItem<ComboItem>.NAME_VALUE);
            
            if (element is CombosToolTreeNode comboToolNode)
            {
                comboToolNode.BindItem(propertyData, id);
            }
        }

        private void OnUnbindItem(VisualElement element, int index)
        {
            if (element is CombosToolTreeNode nodeTool)
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
            if (value is not ComboItem valueCombo) return;

            if (this.m_TreeView.selectedIndex == -1)
            {
                this.CreateAsSelectionSibling(value);
                return;
            }
            
            int parentId = this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex);
            
            ComboTree comboTree = this.CombosTool.Instance;
            int newId = comboTree.AddChild(valueCombo, parentId);
            
            this.CombosTool.Instance = comboTree;

            TreeViewItemData<ComboItem> itemData = new TreeViewItemData<ComboItem>(newId, valueCombo);
            this.m_TreeView.AddItem(itemData, parentId);
            this.m_TreeView.SetSelectionById(newId);
            
            this.EventChange?.Invoke();
        }
        
        public void CreateAsSelectionSibling(object value)
        {
            if (value is not ComboItem valueCombo) return;

            ComboTree comboTree = this.CombosTool.Instance;
            int[] rootIds = this.m_TreeView.GetRootIds()?.ToArray() ?? Array.Empty<int>();
            
            int selectedId = this.m_TreeView.selectedIndex switch
            {
                -1 => rootIds.Length > 0 ? rootIds[^1] : ComboTree.NODE_INVALID,
                _ => this.m_TreeView.GetIdForIndex(this.m_TreeView.selectedIndex)
            };
            
            int newId = selectedId != ComboTree.NODE_INVALID
                ? comboTree.AddAfterSibling(valueCombo, selectedId)
                : comboTree.AddToRoot(valueCombo);
            
            this.CombosTool.Instance = comboTree;

            int parentId = this.m_TreeView.GetParentIdForIndex(this.m_TreeView.selectedIndex);
            int selectedIndex = this.m_TreeView.viewController.GetChildIndexForId(selectedId);

            TreeViewItemData<ComboItem> itemData = new TreeViewItemData<ComboItem>(newId, valueCombo);
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
            
            ComboTree comboTree = this.CombosTool.Instance;
            
            bool success = comboTree.Remove(selectedId);
            this.CombosTool.Instance = comboTree;

            if (!success) return;

            this.m_TreeView.TryRemoveItem(selectedId);

            this.EventChange?.Invoke();
            this.m_TreeView.SetSelection(ComboTree.NODE_INVALID);
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
            
            ComboTree comboTree = this.CombosTool.Instance;

            this.SynchronizeRoots(comboTree);
            this.SynchronizeNodes(comboTree);

            this.CombosTool.Instance = comboTree;
        }
        
        private void SynchronizeRoots(ComboTree comboTree)
        {
            comboTree.RootIds = this.m_TreeView.GetRootIds()?.ToArray() ?? Array.Empty<int>();
        }

        private void SynchronizeNodes(ComboTree comboTree)
        {
            int[] rootIds = this.m_TreeView.GetRootIds()?.ToArray() ?? Array.Empty<int>();

            foreach (int rootId in rootIds)
            {
                SynchronizeNode(comboTree, rootId, ComboTree.NODE_INVALID);
            }
        }

        private void SynchronizeNode(ComboTree comboTree, int node, int ancestor)
        {
            int index = this.m_TreeView.viewController.GetIndexForId(node);
            int[] childrenIds = this.m_TreeView.GetChildrenIdsForIndex(index)?.ToArray() ?? Array.Empty<int>();

            TreeNode value = comboTree.Nodes[node]; 
            value.Children = new List<int>(childrenIds);
            value.Parent = ancestor;
            
            foreach (int childId in childrenIds)
            {
                SynchronizeNode(comboTree, childId, node);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IEnumerable<TreeViewItemData<ComboItem>> GetTree(ComboTree comboTree, int nodeId)
        {
            ComboItem comboItem = comboTree.Get(nodeId);
            if (comboItem == null) return new List<TreeViewItemData<ComboItem>>();
        
            List<int> children = comboTree.Children(nodeId);
            List<TreeViewItemData<ComboItem>> subTree = new List<TreeViewItemData<ComboItem>>();
        
            foreach (int childId in children)
            {
                IEnumerable<TreeViewItemData<ComboItem>> childEntries = this.GetTree(comboTree, childId);
                subTree.AddRange(childEntries);
            }
        
            return new List<TreeViewItemData<ComboItem>>
            {
                new TreeViewItemData<ComboItem>(nodeId, comboItem, subTree)
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