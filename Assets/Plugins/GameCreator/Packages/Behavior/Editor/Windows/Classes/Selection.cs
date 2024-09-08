using System;
using System.Collections.Generic;

namespace GameCreator.Editor.Behavior
{
    internal class Selection
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public TNodeTool Active
        {
            get => this.Group.Length > 0 ? this.Group[0] : null;
            set
            {
                this.SelectGroup(value != null 
                    ? new[] { value } 
                    : Array.Empty<TNodeTool>()
                );
            }
        }

        [field: NonSerialized] public TNodeTool[] Group { get; private set; }

        public bool HasSelection => this.Group.Length >= 1;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChangeSelection;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Selection()
        {
            this.Group = Array.Empty<TNodeTool>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool IsSelected(TNodeTool nodeTool)
        {
            foreach (TNodeTool selected in this.Group)
            {
                if (selected == nodeTool) return true;
            }

            return false;
        }

        public void AddToGroup(TNodeTool nodeTool)
        {
            if (nodeTool == null) return;
            
            foreach (TNodeTool selected in this.Group)
            {
                if (selected == nodeTool) return;
            }

            List<TNodeTool> group = new List<TNodeTool>(this.Group.Length + 1)
            {
                nodeTool
            };
            
            group.AddRange(this.Group);
            this.SelectGroup(group);
        }
        
        public void SelectGroup(IEnumerable<TNodeTool> group)
        {
            foreach (TNodeTool selected in this.Group)
            {
                selected?.OnDeselect();
            }

            List<TNodeTool> newSelection = new List<TNodeTool>();
            foreach (TNodeTool candidate in group)
            {
                if (newSelection.Contains(candidate)) continue;
                newSelection.Add(candidate);
            }
            
            this.Group = newSelection.ToArray();
            foreach (TNodeTool selected in this.Group)
            {
                selected?.OnSelect();
            }
            
            this.EventChangeSelection?.Invoke();
        }

        public void Clear()
        {
            this.Active = null;
        }
    }
}