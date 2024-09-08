using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogue.png")]
    [AddComponentMenu("Game Creator/Dialogue/Dialogue")]
    
    [DisallowMultipleComponent]
    public class Dialogue : MonoBehaviour
    {
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode()
        {
            Current = null;
            
            EventStartLine = null;
            EventFinishLine = null;
        }
        
        #endif
        
        private const string ERR_NO_SKIN = "Failed to run Dialogue: No skin found";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private Story m_Story = new Story();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Story Story => this.m_Story;
        
        public static Dialogue Current { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action<Dialogue> EventStartLine;
        public static event Action<Dialogue> EventFinishLine;
        
        public event Action EventStart;
        public event Action EventFinish;
        
        public event Action<int> EventStartNext;
        public event Action<int> EventFinishNext;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Play(Args args)
        {
            if (this.m_Story.Content.DialogueSkin == null)
            {
                Debug.LogError(ERR_NO_SKIN);
                return;
            }

            Current = this;
            
            await DialogueUI.Open(this.m_Story.Content.DialogueSkin, this, true);
            this.EventStart?.Invoke();

            this.m_Story.EventStartNext -= this.OnStartNext;
            this.m_Story.EventFinishNext -= this.OnFinishNext;
            
            this.m_Story.EventStartNext += this.OnStartNext;
            this.m_Story.EventFinishNext += this.OnFinishNext;
            
            await this.m_Story.Play(args);
            
            this.Stop();
        }

        public void Stop()
        {
            this.m_Story.EventStartNext -= this.OnStartNext;
            this.m_Story.EventFinishNext -= this.OnFinishNext;

            this.m_Story.IsCanceled = true;
            this.EventFinish?.Invoke();

            Current = null;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Reset()
        {
            this.m_Story.Content.EditorReset();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnStartNext(int nodeId)
        {
            this.EventStartNext?.Invoke(nodeId);
            EventStartLine?.Invoke(this);
        }
        
        private void OnFinishNext(int nodeId)
        {
            this.EventFinishNext?.Invoke(nodeId);
            EventFinishLine?.Invoke(this);
        }
    }
}
