using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Change Awareness Level")]
    [Category("Perception/On Change Awareness Level")]
    
    [Description("Executed when the Awareness value of a target changes")]

    [Image(typeof(IconAwareness), ColorTheme.Type.Blue)]
    [Keywords("Perceive", "Alert", "Aware", "Suspicious", "Curious", "Detect")]
    
    [Serializable]
    public class EventPerceptionOnAwarenessLevel : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField] private CompareGameObjectOrAny m_Target = new CompareGameObjectOrAny();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private GameObject m_Source;
        [NonSerialized] private Args m_Args;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            Perception perception = this.m_Perception.Get<Perception>(trigger);
            if (perception == null) return;
            
            this.m_Source = perception.gameObject;
            this.m_Args = new Args(perception.gameObject);

            perception.EventChangeAwarenessLevel -= this.OnChangeAwarenessLevel;
            perception.EventChangeAwarenessLevel += this.OnChangeAwarenessLevel;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;

            Perception perception = this.m_Source != null ? this.m_Source.Get<Perception>() : null;
            if (perception == null) return;
            
            perception.EventChangeAwarenessLevel -= this.OnChangeAwarenessLevel;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChangeAwarenessLevel(GameObject target, float awareness)
        {
            if (this.m_Args.Target != target)
            {
                this.m_Args.ChangeTarget(target);
            }
            
            if (this.m_Target.Match(target, m_Args))
            {
                _ = this.m_Trigger.Execute(this.m_Args);
            }
        }
    }
}