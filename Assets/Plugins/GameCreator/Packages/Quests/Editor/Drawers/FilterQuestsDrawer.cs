using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Quests.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    [CustomPropertyDrawer(typeof(FilterQuests))]
    public class FilterQuestsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty show = property.FindPropertyRelative("m_Show");
            SerializedProperty showHidden = property.FindPropertyRelative("m_ShowHidden");
            SerializedProperty hideUntracked = property.FindPropertyRelative("m_HideUntracked");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(show));
            root.Add(new PropertyField(showHidden));
            root.Add(new PropertyField(hideUntracked));
            
            SerializedProperty filter = property.FindPropertyRelative("m_Filter");
            SerializedProperty localList = property.FindPropertyRelative("m_LocalList");
            SerializedProperty globalList = property.FindPropertyRelative("m_GlobalList");

            PropertyField fieldFilter = new PropertyField(filter);
            PropertyField fieldLocalList = new PropertyField(localList);
            PropertyField fieldGlobalList = new PropertyField(globalList);
            
            root.Add(new SpaceSmaller());
            root.Add(fieldFilter);
            root.Add(fieldLocalList);
            root.Add(fieldGlobalList);

            fieldLocalList.style.display = filter.enumValueIndex switch
            {
                0 => DisplayStyle.None,
                1 => DisplayStyle.Flex,
                2 => DisplayStyle.None,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            fieldGlobalList.style.display = filter.enumValueIndex switch
            {
                0 => DisplayStyle.None,
                1 => DisplayStyle.None,
                2 => DisplayStyle.Flex,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            fieldFilter.RegisterValueChangeCallback(changeEvent =>
            {
                fieldLocalList.style.display = changeEvent.changedProperty.enumValueIndex switch
                {
                    0 => DisplayStyle.None,
                    1 => DisplayStyle.Flex,
                    2 => DisplayStyle.None,
                    _ => throw new ArgumentOutOfRangeException()
                };
            
                fieldGlobalList.style.display = changeEvent.changedProperty.enumValueIndex switch
                {
                    0 => DisplayStyle.None,
                    1 => DisplayStyle.None,
                    2 => DisplayStyle.Flex,
                    _ => throw new ArgumentOutOfRangeException()
                };
            });

            return root;
        }
    }
}