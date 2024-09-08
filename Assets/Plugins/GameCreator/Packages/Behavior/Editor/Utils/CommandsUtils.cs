using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal delegate bool CommandHandle(TGraphTool graphTool, ArgData argData);

    internal static class CommandsUtils
    {
        public static readonly Dictionary<string, CommandHandle> LIST;

        private static readonly Vector2 PASTE_OFFSET = new Vector2(50, 50);

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        static CommandsUtils()
        {
            LIST = new Dictionary<string, CommandHandle>
            {
                { "FrameSelected", FrameSelection },
                { "SelectAll", SelectEverything },
                { "SoftDelete", Delete },
                { "Delete", Delete },
                { "Copy", Copy },
                { "Cut", Cut },
                { "Paste", Paste },
                { "Duplicate", Duplicate },
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static bool FrameSelection(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;
            
            graphTool.FrameSelection();
            return true;
        }
        
        public static bool Delete(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;

            TNodeTool[] selection = graphTool.Window.Selection.Group;
            string[] selectionIds = new string[selection.Length];
            
            graphTool.Window.Selection.Clear();

            for (int i = 0; i < selection.Length; ++i)
            {
                selectionIds[i] = selection[i]?.NodeId;
            }
            
            for (int i = selectionIds.Length - 1; i >= 0; --i)
            {
                string nodeId = selectionIds[i];
                
                if (string.IsNullOrEmpty(nodeId)) continue;
                graphTool.DeleteNode(nodeId);
            }
            
            return true;
        }
        
        public static bool SelectEverything(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;

            Dictionary<string, TNodeTool>.ValueCollection nodeTools = graphTool.NodeTools.Values;
            graphTool.Window.Selection.SelectGroup(nodeTools);
            return true;
        }
        
        public static bool Copy(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;
            
            TNodeTool[] selection = graphTool.Window.Selection.Group;
            if (selection.Length == 0) return false;

            TNodeTool selected = selection[0];
            if (selected is not { CanDelete: true })
            {
                CopyPasteUtils.ClearCopy();
                return false;
            }
            
            CopyPasteUtils.SoftCopy(selected.Node, typeof(TNodeTool));
            return true;
        }
        
        public static bool Cut(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;
        
            TNodeTool[] selection = graphTool.Window.Selection.Group;
            if (selection.Length == 0) return false;

            TNodeTool selected = selection[0];
            if (selected is not { CanDelete: true }) return false;
            
            TNode instance = selected.Node;
            graphTool.Window.Selection.Clear();
            
            CopyPasteUtils.SoftCopy(instance, typeof(TNode));
            graphTool.DeleteNode(instance.Id.String);
            
            return true;
        }
        
        public static bool Paste(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;
        
            if (!CopyPasteUtils.CanSoftPaste(typeof(TNode))) return false;
            if (!graphTool.AcceptNode(CopyPasteUtils.SourceObjectCopy as TNode)) return false;

            argData.Callback = CopyValuesToInstance;
            if (argData.Cursor == Vector2.zero)
            {
                Vector2 position = (CopyPasteUtils.SourceObjectCopy as TNode)?.Position ?? default;
                argData.Cursor = position + PASTE_OFFSET;
            }
            
            graphTool.CreateNode(CopyPasteUtils.SourceObjectCopy.GetType(), argData);
            return true;
        }

        public static bool Duplicate(TGraphTool graphTool, ArgData argData)
        {
            if (graphTool == null) return false;
            return Copy(graphTool, argData) && Paste(graphTool, argData);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private static void CopyValuesToInstance(string portId, TNodeTool newNodeTool)
        {
            object copy = CopyPasteUtils.SourceObjectCopy;
            newNodeTool.PasteValues(copy);
            CopyPasteUtils.ClearCopy();
        }
    }
}