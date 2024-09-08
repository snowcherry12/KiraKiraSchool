using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoPerception.png")]
    [AddComponentMenu("Game Creator/Perception/Perception")]
    
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST)]
    
    [Serializable]
    public class Perception : MonoBehaviour, ISpatialHash
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_CanForget;
        [SerializeField] private PropertyGetDecimal m_Duration = new PropertyGetDecimal(30f);

        [SerializeField] private PropertyGetDecimal m_ForgetSpeed = GetDecimalConstantPointFive.Create;
        [SerializeField] private PropertyGetDecimal m_ForgetDelay = GetDecimalConstantOne.Create;
        
        [SerializeField] private SensorList m_Sensors = new SensorList();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private Cortex m_Cortex = new Cortex();

        // PROPERTIES: ----------------------------------------------------------------------------

        internal float AwareDuration => this.m_CanForget 
            ? (float) this.m_Duration.Get(this.Args)
            : float.MaxValue;
        
        internal float ForgetSpeed => (float) this.m_ForgetSpeed.Get(this.Args);
        internal float ForgetDelay => (float) this.m_ForgetDelay.Get(this.Args);
        
        internal float Time
        {
            get
            {
                Character character = this.Get<Character>();
                return character != null ? character.Time.Time : UnityEngine.Time.time;
            }
        }
        
        internal float DeltaTime
        {
            get
            {
                Character character = this.Get<Character>();
                return character != null ? character.Time.DeltaTime : UnityEngine.Time.deltaTime;
            }
        }

        internal IEnumerable<string> EvidenceTags => this.m_Cortex.Evidences.Keys;
        
        [field: NonSerialized] internal Args Args { get; private set; }
        
        [field: NonSerialized] internal Animator Animator { get; private set; }
        
        public List<Tracker> TrackerList => new List<Tracker>(this.m_Cortex.TrackerList);
        
        [field: NonSerialized] public GameObject LastNoticedEvidence { get; private set; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventAwarenessChange;
        public event Action<GameObject> EventNoticeEvidence;
        public event Action<GameObject> EventRelayedEvidence;
        
        public event Action<GameObject, float> EventChangeAwarenessLevel;
        public event Action<GameObject, AwareStage> EventChangeAwarenessStage;
        public event Action<GameObject> EventRelayedAwareness;
        
        public event Action<GameObject> EventTrack;
        public event Action<GameObject> EventUntrack;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.Args = new Args(this);
            this.Animator = this.GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            SpatialHashPerception.Insert(this);
            
            this.m_Cortex.Enable(this);
            this.m_Sensors.Enable(this);

            this.m_Cortex.EventNoticeEvidence += this.OnNoticeEvidence;
            
            this.m_Cortex.EventAwarenessChangeLevel += this.OnChangeAwarenessLevel;
            this.m_Cortex.EventAwarenessChangeStage += this.OnChangeAwarenessStage;
            
            this.m_Cortex.EventAwarenessTrack += this.OnTrack;
            this.m_Cortex.EventAwarenessUntrack += this.OnUntrack;
        }

        private void OnDisable()
        {
            this.m_Cortex.EventNoticeEvidence -= this.OnNoticeEvidence;
            
            this.m_Cortex.EventAwarenessChangeLevel -= this.OnChangeAwarenessLevel;
            this.m_Cortex.EventAwarenessChangeStage -= this.OnChangeAwarenessStage;
            
            this.m_Cortex.EventAwarenessTrack -= this.OnTrack;
            this.m_Cortex.EventAwarenessUntrack -= this.OnUntrack;
            
            this.m_Sensors.Disable(this);
            this.m_Cortex.Disable(this);
            
            SpatialHashPerception.Remove(this);
        }

        // PRIVATE CALLBACKS: ---------------------------------------------------------------------
        
        private void OnNoticeEvidence(GameObject evidence)
        {
            this.LastNoticedEvidence = evidence;
            this.EventNoticeEvidence?.Invoke(evidence);
        }
        
        private void OnTrack(GameObject target)
        {
            this.EventTrack?.Invoke(target);
            this.EventAwarenessChange?.Invoke();
        }
        
        private void OnUntrack(GameObject target)
        {
            this.EventUntrack?.Invoke(target);
            this.EventAwarenessChange?.Invoke();
        }
        
        private void OnChangeAwarenessLevel(GameObject target, float level)
        {
            this.EventChangeAwarenessLevel?.Invoke(target, level);
            this.EventAwarenessChange?.Invoke();
        }
        
        private void OnChangeAwarenessStage(GameObject target, AwareStage stage)
        {
            this.EventChangeAwarenessStage?.Invoke(target, stage);
        }
        
        // PUBLIC AWARENESS METHODS: --------------------------------------------------------------
        
        public Tracker GetTracker(GameObject target)
        {
            return this.m_Cortex.GetAwareness(target);
        }
        
        public bool SetAwareness(GameObject target, float awareness)
        {
            Tracker tracker = this.m_Cortex.GetAwareness(target);
            if (tracker == null) return false;

            tracker.Awareness = awareness;
            return true;
        }
        
        public bool AddAwareness(GameObject target, float value, float maxAwareness = 1f)
        {
            Tracker tracker = this.GetTracker(target);
            if (tracker == null) return false;

            float newAwareness = Math.Min(tracker.Awareness + value, maxAwareness);
            return tracker.Awareness < maxAwareness && this.SetAwareness(target, newAwareness);
        }
        
        public bool SubtractAwareness(GameObject target, float value)
        {
            Tracker tracker = this.GetTracker(target);
            return tracker != null && this.SetAwareness(target, tracker.Awareness - value);
        }
        
        public void RelayAwareness(Perception other)
        {
            if (other == null) return;
            if (this == other) return;
            
            bool newAwareness = false;

            foreach (Tracker thisTracker in this.m_Cortex.TrackerList)
            {
                if (thisTracker.Target == null) continue;
                Tracker otherTracker = other.GetTracker(thisTracker.Target);
                
                if (otherTracker == null) continue;
                if (otherTracker.Awareness > thisTracker.Awareness) continue;
                
                otherTracker.Awareness = thisTracker.Awareness;
                newAwareness = true;
            }

            if (newAwareness)
            {
                other.EventRelayedAwareness?.Invoke(this.gameObject);
            }
        }
        
        // PUBLIC SENSOR METHODS: -----------------------------------------------------------------

        public T GetSensor<T>() where T : TSensor
        {
            return this.m_Sensors.Get<T>();
        }
        
        // PUBLIC TRACK METHODS: ------------------------------------------------------------------
        
        public void Track(GameObject target)
        {
            this.m_Cortex.Track(this, target);
        }
        
        public void Untrack(GameObject target)
        {
            this.m_Cortex.Untrack(target);
        }
        
        public bool IsTracking(GameObject target)
        {
            return target != null && this.m_Cortex.IsTracking(target);
        }
        
        // PUBLIC EVIDENCE METHODS: ---------------------------------------------------------------

        public bool GetEvidence(string evidenceTag)
        {
            return this.m_Cortex.Evidences.TryGetValue(evidenceTag, out bool value) && value;
        }

        public void SetEvidence(string evidenceTag)
        {
            this.m_Cortex.Evidences[evidenceTag] = true;
        }

        public void InvestigateEvidence(Evidence evidence)
        {
            this.m_Cortex.InvestigateEvidence(evidence);
        }
        
        public void RelayEvidence(Perception other)
        {
            if (other == null) return;
            if (this == other) return;
            
            bool newEvidences = false;
            
            foreach (KeyValuePair<string, bool> entry in this.m_Cortex.Evidences)
            {
                if (entry.Value == false) continue;

                newEvidences = 
                    newEvidences ||
                    !other.m_Cortex.Evidences.TryGetValue(entry.Key, out bool value) || !value;
                
                other.m_Cortex.Evidences[entry.Key] = entry.Value;
            }

            if (newEvidences)
            {
                other.EventRelayedEvidence?.Invoke(this.gameObject);
            }
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            this.m_Sensors.Update();
            this.m_Cortex.Update();
        }
        
        private void FixedUpdate()
        {
            this.m_Sensors.FixedUpdate();
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        private void OnDrawGizmosSelected()
        {
            this.m_Sensors.DrawGizmos(this);
            this.m_Cortex.DrawGizmos(this);
        }
    }
}