using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Icon(EditorPaths.PACKAGES + "Behavior/Editor/Gizmos/GizmoProcessor.png")]
    [AddComponentMenu("Game Creator/Behavior/Processor")]
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_EARLIER)]
    
    [Serializable]
    public class Processor : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Graph m_Graph;
        [SerializeField] private UpdateLoop m_Loop = UpdateLoop.Forever;
        [SerializeField] private UpdateTime m_Update = UpdateTime.EveryFrame;
        [SerializeField] private PropertyGetDecimal m_Interval = new PropertyGetDecimal(0.5f);
        
        [SerializeField] private RuntimeData m_RuntimeData = new RuntimeData();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_LastUpdateTime = -9999f;
        [NonSerialized] private Status m_GraphStatus = Status.Ready;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public Status Status => this.m_GraphStatus;
        
        public RuntimeData RuntimeData => this.m_RuntimeData;

        [field: NonSerialized] public Args Args { get; private set; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventBeforeIteration;
        public event Action EventAfterIteration;

        public event Action EventStart;
        public event Action EventFinish;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        private void Awake()
        {
            this.Args = new Args(this.gameObject, this.gameObject);
            this.m_RuntimeData.OnStartup(this.m_Graph);
        }

        private void OnDestroy()
        {
            if (this.m_Graph == null) return;
            this.m_Graph.Abort(this);
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            switch (this.m_Update)
            {
                case UpdateTime.EveryFrame:
                    this.Run();
                    break;
                
                case UpdateTime.Interval:
                    float interval = (float) this.m_Interval.Get(this.Args);
                    if (Time.unscaledTime >= this.m_LastUpdateTime + interval)
                    {
                        this.Run();
                    }
                    break;
                
                case UpdateTime.Manual:
                default: break;
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Tick()
        {
            if (this.m_Update != UpdateTime.Manual) return;
            this.Run();
        }

        public void Restart()
        {
            this.m_Graph.Abort(this);
            this.RuntimeData.Restart();
            this.m_GraphStatus = Status.Ready;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Run()
        {
            this.EventBeforeIteration?.Invoke();
            this.m_LastUpdateTime = Time.unscaledTime;
            
            if (this.m_Graph == null) return;
            
            if (this.m_GraphStatus == Status.Success || this.m_GraphStatus == Status.Failure)
            {
                if (this.m_Loop == UpdateLoop.Never) return;
                if (this.m_Loop == UpdateLoop.Forever) this.Restart();
            }
            
            if (this.m_GraphStatus != Status.Running) this.EventStart?.Invoke();

            this.m_GraphStatus = this.m_Graph.Run(this);
            
            this.EventAfterIteration?.Invoke();
            if (this.m_GraphStatus != Status.Running) this.EventFinish?.Invoke();
        }
        
        // EDITOR METHODS: ------------------------------------------------------------------------

        #if UNITY_EDITOR
        
        public void EditorSyncData()
        {
            if (AssemblyUtils.IsReloading) return;
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            this.m_RuntimeData.SyncData(this.m_Graph);
        }
        
        #endif
    }
}
