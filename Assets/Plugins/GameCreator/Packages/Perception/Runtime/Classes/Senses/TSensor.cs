using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Perception
{
    [Title("Sensor")]
    
    [Serializable]
    public abstract class TSensor : TPolymorphicItem<TSensor>
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public bool Active { get; set; } = true;
        
        [field: NonSerialized] protected Perception Perception { get; private set; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Enable(Perception perception)
        {
            this.Perception = perception;
        }

        public void Disable(Perception perception)
        {
            this.Perception = perception;
        }
        
        public void Update()
        {
            if (this.Active && this.IsEnabled) this.OnUpdate();
        }
        
        public void FixedUpdate()
        {
            if (this.Active && this.IsEnabled) this.OnFixedUpdate();
        }

        public void DrawGizmos(Perception perception)
        {
            if (this.Active && this.IsEnabled) this.OnDrawGizmos(perception);
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------
        
        protected abstract void OnUpdate();
        
        protected abstract void OnFixedUpdate();

        protected abstract void OnDrawGizmos(Perception perception);
    }
}