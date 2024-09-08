using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomEditor(typeof(Processor), true)]
    public class ProcessorEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Processor";

        private const string NAME_ROOT = "GC-Behavior-Processor-Root";
        private const string NAME_HEAD = "GC-Behavior-Processor-Head";
        private const string NAME_BODY = "GC-Behavior-Processor-Body";
        private const string NAME_FOOT = "GC-Behavior-Processor-Foot";

        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;
        private VisualElement m_Foot;

        private PropertyField m_FieldInterval;

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty PropertyRuntimeData =>
            this.serializedObject.FindProperty("m_RuntimeData");
        
        private SerializedProperty PropertyGraph =>
            this.serializedObject.FindProperty("m_Graph");

        // PAINT METHODS: -------------------------------------------------------------------------

        public override bool UseDefaultMargins() => false;

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement { name = NAME_ROOT };
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new VisualElement { name = NAME_BODY };
            this.m_Foot = new VisualElement { name = NAME_FOOT };
            
            StyleSheet[] styleSheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in styleSheets) this.m_Root.styleSheets.Add(styleSheet);
            
            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);
            this.m_Root.Add(this.m_Foot);
            
            SerializedProperty graph = this.PropertyGraph;
            SerializedProperty loop = this.serializedObject.FindProperty("m_Loop");
            SerializedProperty update = this.serializedObject.FindProperty("m_Update");
            SerializedProperty interval = this.serializedObject.FindProperty("m_Interval");

            PropertyField fieldGraph = new PropertyField(graph);
            PropertyField fieldLoop = new PropertyField(loop);
            PropertyField fieldUpdate = new PropertyField(update);
            this.m_FieldInterval = new PropertyField(interval);
            
            this.m_Head.Add(fieldGraph);
            this.m_Head.Add(fieldLoop);
            this.m_Head.Add(fieldUpdate);
            this.m_Head.Add(this.m_FieldInterval);
            
            this.OnChangeUpdate(new SerializedPropertyChangeEvent { changedProperty = update });
            this.RefreshBody();
            
            this.m_Body.RegisterCallback<DetachFromPanelEvent>(this.OnDetachFromPanel);
        
            fieldUpdate.RegisterValueChangeCallback(this.OnChangeUpdate);
            fieldGraph.RegisterValueChangeCallback(this.OnChangeGraphAsset);
            
            TGraphWindow.EventChangeBlackboard -= this.RefreshBody;
            TGraphWindow.EventChangeBlackboard += this.RefreshBody;
            
            TGraphWindow.EventChangeInspector -= this.RefreshBody;
            TGraphWindow.EventChangeInspector += this.RefreshBody;
            
            LabelButton headButtonRefresh = new LabelButton("Refresh Parameters", this.SyncData);
            this.m_Foot.Add(headButtonRefresh);
            
            return this.m_Root;
        }

        private void OnEnable()
        {
            Processor processor = this.target as Processor;
            if (processor != null) processor.EditorSyncData();

            TGraphWindow.EventChangeBlackboard += this.SyncData;
            EditorApplication.playModeStateChanged += this.OnChangePlayMode;
        }

        private void OnDisable()
        {
            TGraphWindow.EventChangeBlackboard -= this.RefreshBody;
            TGraphWindow.EventChangeInspector -= this.RefreshBody;

            Processor processor = this.target as Processor;
            if (processor != null) processor.EditorSyncData();
            
            TGraphWindow.EventChangeBlackboard -= this.SyncData;
            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnChangeUpdate(SerializedPropertyChangeEvent changeEvent)
        {
            this.m_FieldInterval.style.display = changeEvent.changedProperty.enumValueIndex switch
            {
                (int) UpdateTime.Interval => DisplayStyle.Flex,
                _ => DisplayStyle.None
            };
        }
        
        private void OnChangeGraphAsset(SerializedPropertyChangeEvent changeEvent)
        {
            // Note [20/07/23]: The following code crashes Unity when adding or removing a
            // component where the Processor is (even though the asset is not being changed).
            
            // this.SyncData();
            // this.RefreshBody();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent eventDetach)
        {
            TGraphWindow.EventChangeBlackboard -= this.RefreshBody;
            TGraphWindow.EventChangeInspector -= this.RefreshBody;
        }
        
        private void OnChangePlayMode(PlayModeStateChange state)
        {
            this.RefreshBody();

            Processor processor = this.target as Processor;
            if (processor == null) return;
            
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                processor.RuntimeData.EventChangeParameter -= this.RefreshBody;
                processor.RuntimeData.EventChangeParameter += this.RefreshBody;
            }
            else
            {
                processor.RuntimeData.EventChangeParameter -= this.RefreshBody;
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RefreshBody()
        {
            if (this.m_Body == null) return;

            this.m_Body.Clear();
            this.serializedObject.Update();

            if (this.PropertyGraph.objectReferenceValue == null) return;
            
            PropertyField fieldRuntimeData = new PropertyField(this.PropertyRuntimeData);
            this.m_Body.Add(fieldRuntimeData);
            
            fieldRuntimeData.Bind(this.serializedObject);
        }

        private void SyncData()
        {
            Processor processor = this.target as Processor;
            if (processor == null) return;
            
            processor.EditorSyncData();
            this.serializedObject.Update();
            
            this.RefreshBody();
        }
    }
}