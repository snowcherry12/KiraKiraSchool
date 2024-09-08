using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    public class Cortex
    {
        private static readonly Color GIZMO_COLOR_AWARE_NONE = new Color(1f, 1f, 1f, 0.25f);
        private static readonly Color GIZMO_COLOR_AWARE_SUSPICIOUS = new Color(1f, 1f, 0f, 0.5f);
        private static readonly Color GIZMO_COLOR_AWARE_ALERT = new Color(1f, 0f, 0f, 0.5f);
        private static readonly Color GIZMO_COLOR_AWARE_AWARE = new Color(0f, 1f, 0f, 1f);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized]
        private Dictionary<int, Tracker> Trackers { get; } = new Dictionary<int, Tracker>();
        
        [field: NonSerialized]
        public Dictionary<string, bool> Evidences { get; } = new Dictionary<string, bool>();
        
        [field: NonSerialized] private Perception Perception { get; set; }

        public Dictionary<int, Tracker>.ValueCollection TrackerList => this.Trackers.Values;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<GameObject> EventAwarenessTrack;
        public event Action<GameObject> EventAwarenessUntrack;
        
        public event Action<GameObject, float> EventAwarenessChangeLevel;
        public event Action<GameObject, AwareStage> EventAwarenessChangeStage;

        public event Action<GameObject> EventNoticeEvidence; 
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public void Enable(Perception perception)
        {
            this.Perception = perception;
            foreach (KeyValuePair<int, Tracker> entry in this.Trackers)
            {
                entry.Value.Enable();
            }
        }
        
        public void Disable(Perception perception)
        {
            this.Perception = perception;
            foreach (KeyValuePair<int, Tracker> entry in this.Trackers)
            {
                entry.Value.Disable();
            }
        }

        // PUBLIC TRACKING METHODS: ---------------------------------------------------------------

        public void Track(Perception perception, GameObject target)
        {
            if (target == null) return;
            if (this.IsTracking(target)) return;
            
            Tracker tracker = new Tracker(perception, target);
            
            tracker.EventChangeAwareness += this.OnAwarenessChangeAwareness;
            tracker.EventChangeStage += this.OnAwarenessChangeStage;
            
            this.Trackers.Add(target.GetInstanceID(), tracker);
            this.EventAwarenessTrack?.Invoke(target);
        }

        public void Untrack(GameObject target)
        {
            if (target == null) return;
            if (this.Trackers.Remove(target.GetInstanceID(), out Tracker awareness))
            {
                awareness.EventChangeAwareness -= this.OnAwarenessChangeAwareness;
                awareness.EventChangeStage -= this.OnAwarenessChangeStage;
                
                this.EventAwarenessUntrack?.Invoke(target);
            }
        }
        
        public bool IsTracking(GameObject target)
        {
            return target != null && this.Trackers.ContainsKey(target.GetInstanceID());
        }
        
        // PUBLIC AWARENESS METHODS: --------------------------------------------------------------
        
        public Tracker GetAwareness(GameObject target)
        {
            if (target == null) return null;
            
            int targetId = target.GetInstanceID();
            return this.Trackers.GetValueOrDefault(targetId);
        }

        // PUBLIC EVIDENCE METHODS: ---------------------------------------------------------------
        
        public void InvestigateEvidence(Evidence evidence)
        {
            if (evidence == null) return;
            
            string evidenceTag = evidence.GetTag(this.Perception.gameObject);
            bool isTampered = this.Evidences.TryGetValue(evidenceTag, out bool value) && value;
            
            if (evidence.IsTampered == isTampered) return;
            
            this.Evidences[evidenceTag] = evidence.IsTampered;
            if (evidence.IsTampered) this.EventNoticeEvidence?.Invoke(evidence.gameObject);
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------
        
        public void Update()
        {
            foreach (KeyValuePair<int, Tracker> entry in this.Trackers)
            {
                entry.Value.Update();
            }
        }
        
        // PRIVATE CALLBACKS: ---------------------------------------------------------------------
        
        private void OnAwarenessChangeAwareness(GameObject target, float level)
        {
            this.EventAwarenessChangeLevel?.Invoke(target, level);
        }
        
        private void OnAwarenessChangeStage(GameObject target, AwareStage stage)
        {
            this.EventAwarenessChangeStage?.Invoke(target, stage);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public void DrawGizmos(Perception perception)
        {
            foreach (KeyValuePair<int, Tracker> entry in this.Trackers)
            {
                if (entry.Value.Target == null) continue;
                
                Vector3 source = perception.transform.position;
                Vector3 target = entry.Value.Target.transform.position;

                Gizmos.color = entry.Value.Stage switch
                {
                    AwareStage.None => GIZMO_COLOR_AWARE_NONE,
                    AwareStage.Suspicious => GIZMO_COLOR_AWARE_SUSPICIOUS,
                    AwareStage.Alert => GIZMO_COLOR_AWARE_ALERT,
                    AwareStage.Aware => GIZMO_COLOR_AWARE_AWARE,
                    _ => throw new ArgumentOutOfRangeException()
                };

                Gizmos.DrawLine(source, target);
            }
        }
    }
}