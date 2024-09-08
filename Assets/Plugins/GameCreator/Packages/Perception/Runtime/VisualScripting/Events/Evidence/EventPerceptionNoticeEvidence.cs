using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("On Notice Evidence")]
    [Category("Perception/Evidence/On Notice Evidence")]
    
    [Description("Executed when an agent with Perception notices a new Evidence component")]

    [Image(typeof(IconEvidence), ColorTheme.Type.Green)]
    [Keywords("See", "Detect")]
    
    [Serializable]
    public class EventPerceptionNoticeEvidence : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField] private CompareStringOrAny m_Tag = new CompareStringOrAny(
            true,
            GetStringId.Create("my-evidence-tag")
        );
        
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
            
            perception.EventNoticeEvidence -= this.OnNoticeEvidence;
            perception.EventNoticeEvidence += this.OnNoticeEvidence;
        }
        
        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (ApplicationManager.IsExiting) return;

            Perception perception = this.m_Source != null ? this.m_Source.Get<Perception>() : null;
            if (perception == null) return;

            perception.EventNoticeEvidence -= this.OnNoticeEvidence;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnNoticeEvidence(GameObject gameObject)
        {
            Evidence evidence = gameObject.Get<Evidence>();
            string evidenceTag = evidence.GetTag(this.m_Source);
            
            if (this.m_Args.Target != gameObject)
            {
                this.m_Args.ChangeTarget(gameObject);
            }

            if (this.m_Tag.Match(evidenceTag, this.m_Args))
            {
                _ = this.m_Trigger.Execute(this.m_Args);
            }
        }
    }
}