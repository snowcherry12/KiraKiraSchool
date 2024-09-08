using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class PickTaskTool : VisualElement
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Quests/Editor/StyleSheets/PickTaskTool";

        private const string EMPTY_LABEL = " ";
        private const string NAME_ROOT_NAME = "GC-Quests-PickTask-Name";
        private const string NAME_DROPDOWN = "GC-Quests-PickTask-Dropdown";

        private static readonly IIcon ICON_DROPDOWN = new IconArrowDropDown(ColorTheme.Type.TextLight);
        
        // STRUCTS: -------------------------------------------------------------------------------
        
        private struct Entry
        {
            public int id;
            public string name;
            public bool enabled;
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly SerializedProperty m_Property;
        private readonly SerializedProperty m_PropertyQuest;
        private readonly SerializedProperty m_PropertyTaskId;
        
        private readonly VisualElement m_Head;
        private readonly VisualElement m_Body;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public PickTaskTool(SerializedProperty property)
        {
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in sheets) this.styleSheets.Add(styleSheet);

            this.Add(this.m_Head);
            this.Add(this.m_Body);

            this.m_Property = property;
            this.m_PropertyQuest = this.m_Property.FindPropertyRelative("m_Quest");
            this.m_PropertyTaskId = this.m_Property.FindPropertyRelative("m_TaskId");

            PropertyField fieldQuest = new PropertyField(this.m_PropertyQuest);
            this.m_Head.Add(fieldQuest);

            fieldQuest.RegisterValueChangeCallback(this.OnChangeQuest);
            this.RefreshTask(this.m_PropertyQuest.objectReferenceValue as Quest);
        }
        
        private void OnChangeQuest(SerializedPropertyChangeEvent changeEvent)
        {
            this.RefreshTask(changeEvent.changedProperty.objectReferenceValue as Quest);
        }
        
        // REFRESH METHODS: -----------------------------------------------------------------------
        
        private void RefreshTask(Quest quest)
        {
            this.m_Body.Clear();
            SerializationUtils.ApplyUnregisteredSerialization(this.m_Property.serializedObject);

            Dictionary<string, Entry> tasksList = this.GetTasksList(quest);
            List<string> choices = new List<string>();
            int index = 0;
            
            foreach (KeyValuePair<string, Entry> entry in tasksList)
            {
                if (entry.Value.id == this.m_PropertyTaskId.intValue) index = choices.Count;
                choices.Add(entry.Value.name);
            }

            DropdownField dropdownField = new DropdownField(
                choices, index,
                entry => entry, entry  => entry
            )
            {
                label = EMPTY_LABEL
            };
            
            AlignLabel.On(dropdownField);
            dropdownField.SetEnabled(quest != null);
            
            dropdownField.RegisterValueChangedCallback(changeEvent =>
            {
                if (!tasksList.TryGetValue(changeEvent.newValue, out Entry entry)) return;
                this.m_PropertyTaskId.intValue = entry.id;
                
                SerializedObject serializedObject = this.m_Property.serializedObject;
                SerializationUtils.ApplyUnregisteredSerialization(serializedObject);
            });
            
            this.m_Body.Add(dropdownField);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Dictionary<string, Entry> GetTasksList(Quest quest)
        {
            Dictionary<string, Entry> list = new Dictionary<string, Entry>
            {
                {
                    string.Empty, new Entry
                    {
                        id = 0,
                        name = string.Empty,
                        enabled = false
                    }
                }
            };
            
            if (quest == null) return list;
            
            TasksTree tasks = quest.Tasks;
            foreach (int rootId in tasks.RootIds)
            {
                Task root = tasks.Get(rootId);
                string key = this.CreateKey(list, root.ToString());
                
                Entry entry = new Entry
                {
                    id = rootId,
                    name = key,
                    enabled = true
                };

                list.Add(key, entry);
                this.GetTasksFrom(quest, list, entry);
            }

            return list;
        }

        private void GetTasksFrom(Quest quest, Dictionary<string, Entry> list, Entry node)
        {
            foreach (int childId in quest.Tasks.Children(node.id))
            {
                Task task = quest.GetTask(childId);
                string key = this.CreateKey(list, $"{node.name}/{task}");
                
                Entry child = new Entry
                {
                    id = childId,
                    name = key,
                    enabled = true
                };
                
                list.Add(key, child);
                this.GetTasksFrom(quest, list, child);
            }
        }

        private string CreateKey(Dictionary<string, Entry> list, string key)
        {
            string newKey = key;
            int counter = 1;
            
            while (list.ContainsKey(newKey))
            {
                counter += 1;
                newKey = $"{key} ({counter})";
            }

            return newKey;
        }
    }
}