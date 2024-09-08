using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(Task))]
    public class TaskDrawer : PropertyDrawer
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Quests/Editor/StyleSheets/Task";
        private const string NAME_COUNTER = "GC-Quests-Tasks-Section-Counter";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) root.styleSheets.Add(sheet);
            
            SerializedProperty type = property.FindPropertyRelative("m_Completion");
            SerializedProperty isHidden = property.FindPropertyRelative("m_IsHidden");

            PropertyField fieldType = new PropertyField(type);
            PropertyField fieldIsHidden = new PropertyField(isHidden);
            
            root.Add(fieldType);
            root.Add(fieldIsHidden);

            SerializedProperty name = property.FindPropertyRelative("m_Name");
            SerializedProperty desc = property.FindPropertyRelative("m_Description");
            SerializedProperty color = property.FindPropertyRelative("m_Color");
            SerializedProperty sprite = property.FindPropertyRelative("m_Sprite");
            
            PropertyField fieldName = new PropertyField(name);
            PropertyField fieldDesc = new PropertyField(desc);
            PropertyField fieldColor = new PropertyField(color);
            PropertyField fieldSprite = new PropertyField(sprite);
            
            root.Add(new SpaceSmall());
            root.Add(fieldName);
            root.Add(new SpaceSmaller());
            root.Add(fieldDesc);
            root.Add(new SpaceSmaller());
            root.Add(fieldColor);
            root.Add(new SpaceSmaller());
            root.Add(fieldSprite);

            SerializedProperty useCounter = property.FindPropertyRelative("m_UseCounter");
            SerializedProperty countTo = property.FindPropertyRelative("m_CountTo");
            SerializedProperty checkWhen = property.FindPropertyRelative("m_CheckWhen");
            SerializedProperty valueFrom = property.FindPropertyRelative("m_ValueFrom");

            VisualElement counterContent = new VisualElement { name = NAME_COUNTER };
            PropertyField fieldUseCounter = new PropertyField(useCounter);
            VisualElement contentCountTo = new VisualElement();
            VisualElement contentProperty = new VisualElement();

            fieldUseCounter.RegisterValueChangeCallback(change =>
            {
                contentCountTo.style.display =
                    change.changedProperty.enumValueIndex == (int) ProgressType.Value ||
                    change.changedProperty.enumValueIndex == (int) ProgressType.Property
                        ? DisplayStyle.Flex
                        : DisplayStyle.None;
                
                contentProperty.style.display =
                    change.changedProperty.enumValueIndex == (int) ProgressType.Property
                        ? DisplayStyle.Flex
                        : DisplayStyle.None;
            });
            
            contentCountTo.style.display =
                useCounter.enumValueIndex == (int) ProgressType.Value ||
                useCounter.enumValueIndex == (int) ProgressType.Property
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            
            contentProperty.style.display =
                useCounter.enumValueIndex == (int) ProgressType.Property
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;

            contentCountTo.Add(new PropertyField(countTo));
            contentProperty.Add(new SpaceSmall());
            contentProperty.Add(new LabelTitle("Detect when:"));
            contentProperty.Add(new SpaceSmaller());
            contentProperty.Add(new PropertyField(checkWhen));
            contentProperty.Add(new PropertyField(valueFrom));
            
            counterContent.Add(fieldUseCounter);
            counterContent.Add(contentCountTo);
            counterContent.Add(contentProperty);

            root.Add(new SpaceSmall());
            root.Add(counterContent);
            
            SerializedProperty onActivate = property.FindPropertyRelative("m_OnActivate");
            SerializedProperty onDeactivate = property.FindPropertyRelative("m_OnDeactivate");
            SerializedProperty onComplete = property.FindPropertyRelative("m_OnComplete");
            SerializedProperty onAbandon = property.FindPropertyRelative("m_OnAbandon");
            SerializedProperty onFail = property.FindPropertyRelative("m_OnFail");

            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Activate:"));
            root.Add(new PropertyField(onActivate));
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Deactivate:"));
            root.Add(new PropertyField(onDeactivate));
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Complete:"));
            root.Add(new PropertyField(onComplete));
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Abandon:"));
            root.Add(new PropertyField(onAbandon));
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Fail:"));
            root.Add(new PropertyField(onFail));

            return root;
        }
    }
}