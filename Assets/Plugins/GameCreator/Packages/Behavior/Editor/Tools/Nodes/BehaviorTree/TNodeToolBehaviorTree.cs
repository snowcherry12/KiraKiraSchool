using System;
using System.Collections.Generic;
using GameCreator.Runtime.Behavior;
using UnityEditor;

namespace GameCreator.Editor.Behavior
{
    internal abstract class TNodeToolBehaviorTree : TNodeTool
    {
        protected TNodeToolBehaviorTree(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override string GetPortText(string portId)
        {
            foreach (KeyValuePair<string, TNodeTool> entry in this.GraphTool.NodeTools)
            {
                TNodeTool nodeTool = entry.Value;
                if (nodeTool == null) continue;
                
                foreach (TPortTool outputPortTool in nodeTool.OutputPortTools)
                {
                    SerializedProperty connections = outputPortTool.Connections;
                    int connectionsSize = connections.arraySize;
                    
                    for (int i = 0; i < connectionsSize; ++i)
                    {
                        SerializedProperty connection = connections.GetArrayElementAtIndex(i);
                        string connectionValue = connection
                            .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                            .stringValue;
                        
                        if (connectionValue == portId)
                        {
                            return outputPortTool.Port.Allowance switch
                            {
                                PortAllowance.Single => string.Empty,
                                PortAllowance.Multiple => (i + 1).ToString(),
                                _ => throw new ArgumentOutOfRangeException()
                            };
                        }
                    }
                }
            }

            return string.Empty;
        }

        protected override TPortTool CreatePort(TNodeTool nodeTool, SerializedProperty property)
        {
            return new PortToolBehaviorTree(nodeTool, property);
        }

        public override void OnMoveChildren()
        {
            base.OnMoveChildren();
            
            foreach (TPortTool outputPortTool in this.OutputPortTools)
            {
                SerializedProperty connections = outputPortTool.Connections;
                TPortTool[] connectionTools = new TPortTool[connections.arraySize];

                for (int i = 0; i < connections.arraySize; ++i)
                {
                    string connectionPortId = connections
                        .GetArrayElementAtIndex(i)
                        .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                        .stringValue;
                    
                    if (string.IsNullOrEmpty(connectionPortId)) continue;
                    TPortTool childPort = this.GraphTool.PortTools[connectionPortId];
                    
                    connectionTools[i] = childPort;
                }
                
                Array.Sort(connectionTools, SortByPosition);
                
                string[] portIds = new string[connectionTools.Length];
                for (int i = 0; i < connectionTools.Length; ++i)
                {
                    portIds[i] = connectionTools[i].PortId;
                }

                for (int i = 0; i < connections.arraySize; ++i)
                {
                    connections
                        .GetArrayElementAtIndex(i)
                        .FindPropertyRelative(ConnectionDrawer.PROP_VALUE)
                        .stringValue = portIds[i];
                }
            }
            
            this.Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            this.Property.serializedObject.Update();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private static int SortByPosition(TPortTool portTool1, TPortTool portTool2)
        {
            return portTool1.NodeTool.Position.x.CompareTo(portTool2.NodeTool.Position.x);
        }
    }
}