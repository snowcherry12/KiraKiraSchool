using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    public abstract class GraphEditor : UnityEditor.Editor
    {
        private static readonly Length PADDING = new Length(5, LengthUnit.Pixel);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract string TypeName { get; }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            this.EditorResolveDuplicates();
        }

        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            Button buttonOpen = new Button(this.OpenGraph)
            {
                text = $"Open {this.TypeName}",
                style = 
                {
                    paddingTop = PADDING,
                    paddingBottom = PADDING
                }
            };
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(buttonOpen);

            return this.m_Root;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected abstract void OpenGraph();
        
        // DUPLICATE FIX METHODS: -----------------------------------------------------------------
        
        private void EditorResolveDuplicates()
        {
            Graph graph = this.target as Graph;
            if (graph == null) return;

            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            string[] candidateGuids = AssetDatabase.FindAssets($"t:{graph.GetType()}");
            foreach (string candidateGuid in candidateGuids)
            {
                string candidatePath = AssetDatabase.GUIDToAssetPath(candidateGuid);
                Graph candidate = AssetDatabase.LoadAssetAtPath<Graph>(candidatePath);
                
                if (candidate == null) continue;
                if (candidate == graph) continue;

                if (EditorIsDuplicate(graph, candidate))
                {
                    this.EditorFixDuplicates();

                    return;
                }
            }
        }

        private void EditorFixDuplicates()
        {
            Graph graph = this.target as Graph;
            if (graph == null) return;

            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            Dictionary<string, string> inputPortIds = new Dictionary<string, string>();
            
            foreach (TNode node in graph.Nodes)
            {
                foreach (TInputPort inputPort in node.Ports.Inputs)
                {
                    string newId = UniqueID.GenerateID();
                    inputPortIds[inputPort.Id.Value] = newId;
                }
            }
            
            SerializedProperty nodes = this.serializedObject.FindProperty("m_Nodes");
            
            for (int i = 0; i < nodes.arraySize; ++i)
            {
                SerializedProperty node = nodes.GetArrayElementAtIndex(i);
                
                node.FindPropertyRelative("m_Id")
                    .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                    .stringValue = UniqueID.GenerateID();

                SerializedProperty ports = node.FindPropertyRelative("m_Ports");
                SerializedProperty inputPorts = ports.FindPropertyRelative("m_Inputs");
                SerializedProperty outputPorts = ports.FindPropertyRelative("m_Outputs");
                
                for (int inputIndex = 0; inputIndex < inputPorts.arraySize; ++inputIndex)
                {
                    SerializedProperty inputId = inputPorts
                            .GetArrayElementAtIndex(inputIndex)
                            .FindPropertyRelative("m_Id")
                            .FindPropertyRelative(ConnectionDrawer.PROP_VALUE);
                    
                    inputId.stringValue = inputPortIds[inputId.stringValue];
                }

                for (int outputIndex = 0; outputIndex < outputPorts.arraySize; ++outputIndex)
                {
                    SerializedProperty outputPort = outputPorts.GetArrayElementAtIndex(outputIndex);
                    SerializedProperty outputId = outputPort
                        .FindPropertyRelative("m_Id")
                        .FindPropertyRelative(ConnectionDrawer.PROP_VALUE);
                    
                    outputId.stringValue = UniqueID.GenerateID();
                    SerializedProperty connections = outputPort.FindPropertyRelative("m_Connections");

                    for (int index = 0; index < connections.arraySize; ++index)
                    {
                        SerializedProperty connectionId = connections
                            .GetArrayElementAtIndex(index)
                            .FindPropertyRelative(ConnectionDrawer.PROP_VALUE);

                        string prevInputId = connectionId.stringValue;
                        if (inputPortIds.TryGetValue(prevInputId, out string newInputId))
                        {
                            connectionId.stringValue = newInputId;
                        }
                    }
                }
            }

            this.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static bool EditorIsDuplicate(Graph node, Graph other)
        {
            HashSet<IdString> otherIds = new HashSet<IdString>(other.Nodes.Length);
            foreach (TNode otherNode in other.Nodes)
            {
                otherIds.Add(otherNode.Id);
            }

            foreach (TNode aNode in node.Nodes)
            {
                if (otherIds.Contains(aNode.Id)) return true;
            }

            return false;
        }
    }
}