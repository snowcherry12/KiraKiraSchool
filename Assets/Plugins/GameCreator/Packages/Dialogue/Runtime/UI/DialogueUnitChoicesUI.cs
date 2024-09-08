using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueUI.png")]
    [AddComponentMenu("Game Creator/UI/Dialogue/Unit Choices UI")]
    
    public class DialogueUnitChoicesUI : TDialogueUnitUI
    {
        private const string ERR_NULL_CONTENT = "Null 'Content Choice' in Choices UI component";
        private const string ERR_NULL_PREFAB = "Null 'Prefab Choice' in Choices UI component";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_Active;

        [SerializeField] private RectTransform m_ContentChoice;
        [SerializeField] private GameObject m_PrefabChoice;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Story m_Story;
        [NonSerialized] private int m_NodeId;
        [NonSerialized] private Args m_Args;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnAwake(DialogueUI dialogueUI)
        {
            base.OnAwake(dialogueUI);
            if (this.m_Active != null) this.m_Active.SetActive(false);
        }

        public override void OnReset(bool isNew)
        { }

        public override void OnStartNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;
            
            this.m_Story = story;
            this.m_NodeId = nodeId;
            this.m_Args = args;
            
            Node node = story.Content.Get(this.m_NodeId);
            if (node == null) return;
            
            if (this.m_Active != null) this.m_Active.SetActive(false);

            node.EventStartChoice -= this.OnStartChoice;
            node.EventFinishChoice -= this.OnFinishChoice;
            
            node.EventStartChoice += this.OnStartChoice;
            node.EventFinishChoice += this.OnFinishChoice;
        }
        
        public override void OnFinishNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;
            
            Node node = story.Content.Get(nodeId);
            if (node == null) return;
            
            if (this.m_Active != null) this.m_Active.SetActive(false);
            
            node.EventStartChoice -= this.OnStartChoice;
            node.EventFinishChoice -= this.OnFinishChoice;
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnStartChoice(int nodeId)
        {
            if (ApplicationManager.IsExiting) return;
            
            if (this.m_Active != null) this.m_Active.SetActive(true);
            
            if (this.m_ContentChoice == null) Debug.LogError(ERR_NULL_CONTENT);
            if (this.m_PrefabChoice == null) Debug.LogError(ERR_NULL_PREFAB);

            Node node = this.m_Story?.Content.Get(nodeId);
            if (node?.NodeType is not NodeTypeChoice nodeTypeChoice) return;
            
            for (int i = this.m_ContentChoice.childCount - 1; i >= 0; --i)
            {
                Destroy(this.m_ContentChoice.GetChild(i).gameObject);
            }
            
            List<int> choices = nodeTypeChoice.GetChoices(
                this.m_Story, nodeId, 
                this.m_Args, false
            );

            Button selection = null;
            for (int i = 0; i < choices.Count; i++)
            {
                int choiceId = choices[i];
                GameObject instance = UIUtils.Instantiate(
                    this.m_PrefabChoice,
                    this.m_ContentChoice
                );

                DialogueChoiceUI choiceUI = instance.Get<DialogueChoiceUI>();
                Node choice = this.m_Story.Content.Get(choiceId);

                if (choiceUI != null && choice != null)
                {
                    choiceUI.Setup(i, choiceId, choice, this.m_Story, this.m_Args, this);
                }

                if (selection == null && choiceUI.Button != null && choiceUI.Button.interactable)
                {
                    selection = choiceUI.Button;
                }
            }

            if (this.m_ContentChoice.childCount <= 0) return;
            if (selection != null) selection.Select();
        }

        private void OnFinishChoice(int nodeId)
        {
            if (ApplicationManager.IsExiting) return;
            
            if (this.m_Active != null) this.m_Active.SetActive(false);
            
            for (int i = this.m_ContentChoice.childCount - 1; i >= 0; --i)
            {
                Destroy(this.m_ContentChoice.GetChild(i).gameObject);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Choose(int choiceId)
        {
            Node node = this.m_Story.Content.Get(this.m_NodeId);

            if (node?.NodeType is not NodeTypeChoice nodeTypeChoice) return;
            nodeTypeChoice.Choose(choiceId);

            if (this.m_DialogueUI.DialogueSkin != null)
            {
                this.m_DialogueUI.DialogueSkin.PlayClipSubmit();
            }
        }

        public void Select()
        {
            if (this.m_DialogueUI.DialogueSkin != null)
            {
                this.m_DialogueUI.DialogueSkin.PlayClipSelect();
            }
        }
    }
}