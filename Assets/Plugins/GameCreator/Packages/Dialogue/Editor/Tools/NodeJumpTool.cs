using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class NodeJumpTool : VisualElement
    {
        private const string SPACE = " ";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private SerializedProperty m_Jump;
        private SerializedProperty m_JumpTo;
        
        private readonly PropertyField m_FieldJump;
        private readonly PopupField<string> m_FieldJumpTo;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        private ContentTool ContentTool { get; }
        private SerializedProperty Property { get; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public NodeJumpTool(SerializedProperty property, ContentTool contentTool)
        {
            this.Property = property;
            this.ContentTool = contentTool;

            this.m_Jump = property.FindPropertyRelative("m_Jump");
            this.m_JumpTo = property.FindPropertyRelative("m_JumpTo");

            string choice = this.m_JumpTo
                .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                .stringValue;
            
            List<string> choices = this.GetChoices();
            if (!choices.Contains(choice)) choice = string.Empty;

            this.m_FieldJump = new PropertyField(this.m_Jump);
            this.m_FieldJumpTo =  new PopupField<string>(
                SPACE,
                this.GetChoices(),
                choice,
                this.PrintSelection,
                this.PrintListItem
            );

            this.Add(this.m_FieldJump);
            this.Add(this.m_FieldJumpTo);

            this.ContentTool.Tree.EventChangeTag += this.ResetChoices;
            
            this.m_FieldJump.RegisterValueChangeCallback(changeEvent =>
            {
                int option = changeEvent.changedProperty.enumValueIndex;
                bool jumpTo = option == (int) JumpType.Jump;
                
                this.m_FieldJumpTo.style.display = jumpTo 
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });

            this.m_FieldJumpTo.RegisterValueChangedCallback(changeEvent =>
            {
                SerializedProperty jumpProperty = this.m_JumpTo.FindPropertyRelative(IdStringDrawer.NAME_STRING);
                jumpProperty.stringValue = changeEvent.newValue;

                this.m_JumpTo.serializedObject.ApplyModifiedProperties();
                this.m_JumpTo.serializedObject.Update();
            });

            int option = this.m_Jump.enumValueIndex;
            bool jumpTo = option == (int) JumpType.Jump;
            
            this.m_FieldJumpTo.style.display = jumpTo 
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        ~NodeJumpTool()
        {
            if (this.ContentTool?.Tree == null) return;
            this.ContentTool.Tree.EventChangeTag -= this.ResetChoices;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private string PrintSelection(string text)
        {
            return text;
        }
        
        private string PrintListItem(string text)
        {
            return text;
        }
        
        private void ResetChoices()
        {
            List<string> choices = this.GetChoices();
            this.m_FieldJumpTo.choices = choices;
        }
        
        // PRIVATE STATIC METHODS: ----------------------------------------------------------------
        
        private List<string> GetChoices()
        {
            List<string> choices = new List<string> { string.Empty };
            List<Tag> tags = this.ContentTool.Content.GetTags();

            foreach (Tag tag in tags)
            {
                choices.Add(tag.Name.String);
            }

            return choices;
        }
    }
}