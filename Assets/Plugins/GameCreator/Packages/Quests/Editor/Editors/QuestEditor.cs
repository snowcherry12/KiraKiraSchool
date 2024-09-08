using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : UnityEditor.Editor
    {
        private const string CLASS_INSPECTOR_MARGINS = "gc-inspector-margins-x";
        
        private const string ERR_DUPLICATE_ID = "Another Quest has the same ID as this one";

        private const int MARGIN = 10;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;

        private ErrorMessage m_ErrorId;
        private PropertyField m_FieldId;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            StyleSheet[] sheets = StyleSheetUtils.Load();
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            VisualElement header = new VisualElement
            {
                style =
                {
                    marginTop = MARGIN,
                    marginBottom = MARGIN,
                    marginLeft = MARGIN,
                    marginRight = MARGIN,
                }
            };
            
            this.m_Root.Add(header);
            
            SerializedProperty title = this.serializedObject.FindProperty("m_Title");
            SerializedProperty description = this.serializedObject.FindProperty("m_Description");
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            SerializedProperty sprite = this.serializedObject.FindProperty("m_Sprite");
            
            header.Add(new PropertyField(title));
            header.Add(new SpaceSmaller());
            header.Add(new PropertyField(description));
            header.Add(new SpaceSmaller());
            header.Add(new PropertyField(color));
            header.Add(new SpaceSmaller());
            header.Add(new PropertyField(sprite));
            
            SerializedProperty type = this.serializedObject.FindProperty("m_Type");
            SerializedProperty order = this.serializedObject.FindProperty("m_SortOrder");
            
            header.Add(new SpaceSmall());
            header.Add(new PropertyField(type));
            header.Add(new PropertyField(order));
            
            this.m_ErrorId = new ErrorMessage(string.Empty);
            
            SerializedProperty uniqueId = this.serializedObject.FindProperty("m_UniqueId");
            this.m_FieldId = new PropertyField(uniqueId);

            header.Add(new SpaceSmall());
            header.Add(this.m_ErrorId);
            header.Add(this.m_FieldId);
            
            this.RefreshErrorID();
            this.m_FieldId.RegisterValueChangeCallback(_ =>
            {
                this.RefreshErrorID();
            });

            SerializedProperty tasks = this.serializedObject.FindProperty("m_Tasks");
            TasksTool tasksTool = new TasksTool(tasks);
            
            this.m_Root.Add(tasksTool);

            SerializedProperty onActivate = this.serializedObject.FindProperty("m_OnActivate");
            SerializedProperty onDeactivate = this.serializedObject.FindProperty("m_OnDeactivate");
            SerializedProperty onComplete = this.serializedObject.FindProperty("m_OnComplete");
            SerializedProperty onAbandon = this.serializedObject.FindProperty("m_OnAbandon");
            SerializedProperty onFail = this.serializedObject.FindProperty("m_OnFail");

            VisualElement instructions = new VisualElement();
            instructions.AddToClassList(CLASS_INSPECTOR_MARGINS);

            instructions.Add(new SpaceSmall());
            instructions.Add(new LabelTitle("On Activate:"));
            instructions.Add(new PropertyField(onActivate));
            
            instructions.Add(new SpaceSmall());
            instructions.Add(new LabelTitle("On Deactivate:"));
            instructions.Add(new PropertyField(onDeactivate));
            
            instructions.Add(new SpaceSmall());
            instructions.Add(new LabelTitle("On Complete:"));
            instructions.Add(new PropertyField(onComplete));
            
            instructions.Add(new SpaceSmall());
            instructions.Add(new LabelTitle("On Abandon:"));
            instructions.Add(new PropertyField(onAbandon));
            
            instructions.Add(new SpaceSmall());
            instructions.Add(new LabelTitle("On Fail:"));
            instructions.Add(new PropertyField(onFail));
            
            this.m_Root.Add(instructions);

            return this.m_Root;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RefreshErrorID()
        {
            this.serializedObject.Update();
            this.m_ErrorId.style.display = DisplayStyle.None;

            SerializedProperty uniqueID = this.serializedObject.FindProperty("m_UniqueId");
            
            string itemID = uniqueID
                .FindPropertyRelative(UniqueIDDrawer.SERIALIZED_ID)
                .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                .stringValue;

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(Quest)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(path);

                if (quest.Id.String == itemID && quest != this.target)
                {
                    this.m_ErrorId.Text = ERR_DUPLICATE_ID;
                    this.m_ErrorId.style.display = DisplayStyle.Flex;
                    return;
                }
            }
        }
    }
}