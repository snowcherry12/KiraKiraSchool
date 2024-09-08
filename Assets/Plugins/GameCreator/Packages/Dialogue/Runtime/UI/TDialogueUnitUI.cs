using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueUI.png")]
    
    public abstract class TDialogueUnitUI : MonoBehaviour
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] protected DialogueUI m_DialogueUI;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void OnAwake(DialogueUI dialogueUI)
        {
            this.m_DialogueUI = dialogueUI;
        }

        public abstract void OnReset(bool isNew);

        public abstract void OnStartNext(Story story, int nodeId, Args args);

        public abstract void OnFinishNext(Story story, int nodeId, Args args);
    }
}