using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(Journal))]
    public class JournalEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Quests/Editor/StyleSheets/Journal";

        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;

        private VisualElement m_Head;
        private VisualElement m_Body;

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            this.m_Root.Add(EditorApplication.isPlayingOrWillChangePlaymode switch
            {
                false => this.PaintEditor(),
                true => this.PaintRuntime(),
            });

            return this.m_Root;
        }
        
        // EDITOR METHODS: ------------------------------------------------------------------------

        private VisualElement PaintEditor()
        {
            this.m_Body = new VisualElement();
            this.PaintInspector(true);
            
            return this.m_Body;
        }
        
        // RUNTIME METHODS: -----------------------------------------------------------------------
        
        private VisualElement PaintRuntime()
        {
            this.m_Body = new VisualElement();
            this.RefreshRuntime();
            
            Journal journal = this.target as Journal;
            if (journal == null) return this.m_Body;
            
            journal.EventQuestChange -= this.OnQuestChange;
            journal.EventTaskChange -= this.OnTaskChange;
                
            journal.EventQuestChange += this.OnQuestChange;
            journal.EventTaskChange += this.OnTaskChange;

            return this.m_Body;
        }

        private void RefreshRuntime()
        {
            this.m_Body.Clear();
            this.PaintInspector(false);
            
            this.m_Body.Add(new SpaceSmaller());
            this.m_Body.Add(new LabelTitle("Quests:"));
            
            Journal journal = this.target as Journal;
            if (journal == null) return;

            QuestEntries entries = journal.QuestEntries;
            if (entries.Count == 0)
            {
                this.m_Body.Add(new InfoMessage("No active Quests yet"));
            }
            
            foreach (KeyValuePair<IdString, QuestEntry> entry in entries)
            {
                JournalQuestTool quest = new JournalQuestTool(journal, entry.Key, entry.Value);
                this.m_Body.Add(quest);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void PaintInspector(bool enabled)
        {
            SerializedProperty trackMode = this.serializedObject.FindProperty("m_TrackMode");
            PropertyField fieldTrackMode = new PropertyField(trackMode, "Track");
            fieldTrackMode.SetEnabled(enabled);
            
            this.m_Body.Add(fieldTrackMode);
        }

        // CALLBACKS: -----------------------------------------------------------------------------
        
        private void OnQuestChange(Quest quest) => this.RefreshRuntime();
        private void OnTaskChange(Quest quest) => this.RefreshRuntime();
    }
}