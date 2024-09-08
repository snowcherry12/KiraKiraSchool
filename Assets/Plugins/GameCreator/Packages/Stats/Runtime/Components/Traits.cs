using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Creator/Stats/Traits")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoTraits.png")]
    public class Traits : MonoBehaviour, ISerializationCallbackReceiver
    {
        private const string ERR_NO_CLASS = "Traits component has no Class reference";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Class m_Class;

        [SerializeField]
        private OverrideAttributes m_OverrideAttributes = new OverrideAttributes();

        [SerializeField]
        private OverrideStats m_OverrideStats = new OverrideStats();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private RuntimeStats m_RuntimeStats;
        [NonSerialized] private RuntimeAttributes m_RuntimeAttributes;
        [NonSerialized] private RuntimeStatusEffects m_RuntimeStatusEffects;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Class Class => this.m_Class;

        public RuntimeStats RuntimeStats
        {
            get
            {
                this.RuntimeInitStats();
                return this.m_RuntimeStats;
            }
        }

        public RuntimeAttributes RuntimeAttributes
        {
            get
            {
                this.RuntimeInitAttributes();
                return this.m_RuntimeAttributes;
            }
        }
        
        public RuntimeStatusEffects RuntimeStatusEffects
        {
            get
            {
                this.RuntimeInitStatusEffects();
                return this.m_RuntimeStatusEffects;
            }
        }
        
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventChange;
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            this.RuntimeStatusEffects.Update();
        }

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.RuntimeInitStats();
            this.RuntimeInitAttributes();
            this.RuntimeInitStatusEffects();
        }

        private void RuntimeInitStats()
        {
            if (this.m_RuntimeStats != null) return;
            
            if (this.m_Class == null)
            {
                this.m_RuntimeStats = new RuntimeStats(this);
                Debug.LogError(ERR_NO_CLASS, this);
                return;
            }
            
            this.m_RuntimeStats = new RuntimeStats(this, this.m_OverrideStats);
            this.m_RuntimeStats.EventChange += _ => this.EventChange?.Invoke();
        }
        
        private void RuntimeInitAttributes()
        {
            if (this.m_RuntimeAttributes != null) return;

            if (this.m_Class == null)
            {
                this.m_RuntimeAttributes = new RuntimeAttributes(this);
                Debug.LogError(ERR_NO_CLASS, this);
                return;
            }
            
            this.m_RuntimeAttributes = new RuntimeAttributes(this, this.m_OverrideAttributes);
            this.m_RuntimeAttributes.EventChange += _ => this.EventChange?.Invoke();
        }

        private void RuntimeInitStatusEffects()
        {
            if (this.m_RuntimeStatusEffects != null) return;

            this.m_RuntimeStatusEffects = new RuntimeStatusEffects(this);
            this.m_RuntimeStatusEffects.EventChange += _ => this.EventChange?.Invoke();
        }
        
        // STATIC METHODS: ------------------------------------------------------------------------

        public static Traits SetTraitsWithClass(GameObject target, Class assetClass)
        {
            Traits traits = target.Add<Traits>();
            traits.m_Class = assetClass;

            traits.m_RuntimeStats = null;
            traits.m_RuntimeAttributes = null;
            
            traits.RuntimeInitStats();
            traits.RuntimeInitAttributes();

            return traits;
        }

        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            if (this.m_Class == null) return;

            this.m_OverrideAttributes.SyncWithClass(this.m_Class);
            this.m_OverrideStats.SyncWithClass(this.m_Class);
        }
    }
}