using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    public class SmellManager : Singleton<SmellManager>
    {
        private class Scents
        {
            private const HideFlags FLAGS = HideFlags.HideAndDontSave;
            
            // PROPERTIES: ------------------------------------------------------------------------

            private string Tag { get; }

            private Transform Container { get; }
            private Dictionary<int, int> PreviousId { get; }
            private Queue<Scent> ReadyScents { get; }
            
            public Dictionary<int, Scent> RunningScents { get; }

            // CONSTRUCTOR: -----------------------------------------------------------------------
            
            public Scents(string tag)
            {
                this.Tag = tag;

                this.PreviousId = new Dictionary<int, int>();
                this.ReadyScents = new Queue<Scent>();
                this.RunningScents = new Dictionary<int, Scent>();

                GameObject container = new GameObject(tag) { hideFlags = FLAGS };
                this.Container = container.transform;
            }
            
            // PUBLIC METHODS: --------------------------------------------------------------------

            public void Emit(GameObject source, float duration, float level)
            {
                if (source == null) return;
                int sourceId = source.GetInstanceID();
                
                if (this.ReadyScents.Count <= 0)
                {
                    Scent newScent = Scent.Create(
                        this.Tag,
                        this.Container, 
                        source
                    );
                    
                    this.ReadyScents.Enqueue(newScent);
                }

                Scent scent = this.ReadyScents.Dequeue();
                int scentId = scent.GetInstanceID();
                
                this.RunningScents.Add(scentId, scent);

                this.PreviousId.TryGetValue(sourceId, out int previousId);
                if (this.RunningScents.TryGetValue(previousId, out Scent previous))
                {
                    previous.NextScentId = scentId;
                }
                
                scent.Run(duration, level, this.OnComplete);
                this.PreviousId[sourceId] = scentId;
            }

            private void OnComplete(Scent scent)
            {
                int scentId = scent.GetInstanceID();
                
                this.RunningScents.Remove(scentId);
                this.ReadyScents.Enqueue(scent);
            }
        }

        public Scent GetScent(string scentTag, int scentId)
        {
            if (this.m_Scents.TryGetValue(scentTag, out Scents scents))
            {
                return scents.RunningScents.TryGetValue(scentId, out Scent scent)
                    ? scent
                    : null;
            }

            return null;
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        
        private const float GIZMO_SCENT_MAX_ALPHA = 0.25f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dictionary<PropertyName, Scents> m_Scents;
        
        [NonSerialized] private float m_Dissipation;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float Dissipation
        {
            get => this.m_Dissipation;
            set
            {
                this.m_Dissipation = Math.Max(value, 0f);
                this.EventChangeDissipation?.Invoke();
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        private event Action EventChangeDissipation;
        
        // ON CREATE: -----------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            this.m_Scents = new Dictionary<PropertyName, Scents>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Emit(GameObject source, string scentTag, float duration, float level)
        {
            if (!this.m_Scents.ContainsKey(scentTag))
            {
                Scents scents = new Scents(scentTag);
                this.m_Scents.Add(scentTag, scents);
            }
            
            this.m_Scents[scentTag].Emit(source, duration, level);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        private void OnDrawGizmosSelected()
        {
            foreach (KeyValuePair<PropertyName, Scents> entry in this.m_Scents)
            {
                foreach (KeyValuePair<int, Scent> scents in entry.Value.RunningScents)
                {
                    float alpha = Mathf.Clamp01(1f - scents.Value.Ratio);
                    Gizmos.color = new Color(1f, 0f, 0f, alpha * GIZMO_SCENT_MAX_ALPHA);
                    Gizmos.DrawCube(scents.Value.transform.position, Vector3.one * 0.1f);
                }
            }
            
            foreach (KeyValuePair<PropertyName, Scents> entry in this.m_Scents)
            {
                foreach (KeyValuePair<int, Scent> scents in entry.Value.RunningScents)
                {
                    float alpha = Mathf.Clamp01(1f - scents.Value.Ratio);
                    Gizmos.color = new Color(0f, 1f, 0f, alpha * GIZMO_SCENT_MAX_ALPHA);
                    
                    if (scents.Value == null) continue;
                    Scent scent = scents.Value;
                    
                    if (scent.NextScentId == Scent.SCENT_ID_NONE) continue;

                    bool hasNext = entry.Value.RunningScents.TryGetValue(
                        scent.NextScentId,
                        out Scent nextScent
                    );
                    
                    if (!hasNext || nextScent == null) continue;
                    if (scent.Source != nextScent.Source) continue;
                    
                    Gizmos.DrawLine(
                        scent.transform.position,
                        nextScent.transform.position
                    );
                }
            }
        }
    }
}