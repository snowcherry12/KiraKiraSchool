using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [AddComponentMenu("Game Creator/Melee/Striker")]
    [Icon(RuntimePaths.PACKAGES + "Melee/Editor/Gizmos/GizmoStriker.png")]
    
    public class Striker : MonoBehaviour
    {
        private static readonly List<StrikeOutput> EMPTY = new List<StrikeOutput>();

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private UniqueID m_ID = new UniqueID("striker-id");
        
        [SerializeField] private LayerMask m_LayerMask = -1;
        [SerializeField] private PropertyGetGameObject m_Section = GetGameObjectSelf.Create();
        
        [SerializeReference] private TStrikerShape m_Shape = new StrikerSphere();
        
        [SerializeField] private Trail m_Trail = new Trail();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;

        // PROPERTIES: ----------------------------------------------------------------------------

        public int Id => this.m_ID.Get.Hash;
        
        [field: NonSerialized] private bool CanUpdate { get; set; }
        
        [field: NonSerialized] public Skill Skill { get; internal set; }

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Reset()
        {
            this.m_Section = GetGameObjectInstance.Create(this.gameObject);
        }

        private void Awake()
        {
            Character character = this.gameObject.GetComponentInParent<Character>();
            GameObject characterObject = character != null ? character.gameObject : null;
            
            this.m_Args = new Args(this.gameObject, characterObject);
            this.m_Trail.Awake(this);
        }

        private void OnEnable()
        {
            this.CanUpdate = true;
            this.m_Trail.OnEnable();
        }

        private void OnDisable()
        {
            this.CanUpdate = false;
            this.m_Trail.OnDisable();
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void LateUpdate()
        {
            this.m_Trail.Update();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OnBegin(Character character, Skill skill)
        {
            Transform section = this.m_Section.Get<Transform>(this.m_Args);
            if (section == null) return;
            
            this.m_Shape.Start(section);
            this.m_Trail.Start(character, skill);
        }

        public void OnStop()
        {
            Transform section = this.m_Section.Get<Transform>(this.m_Args);
            if (section == null) return;
            
            this.m_Shape.Stop(section);
            this.m_Trail.Stop();
        }

        public List<StrikeOutput> OnUpdate(int predictions)
        {
            if (!this.CanUpdate) return EMPTY;
            Transform section = this.m_Section.Get<Transform>(this.m_Args);
            
            return section != null 
                ? this.m_Shape.Collect(section, this.m_LayerMask, predictions)
                : EMPTY;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        private void OnDrawGizmosSelected()
        {
            GameObject target = this.GetGizmoTarget();
            if (target != null)
            {
                this.m_Shape.OnDrawGizmos(target.transform);
            }
            
            this.m_Trail.OnDrawGizmos(this);
        }

        private GameObject GetGizmoTarget()
        {
            if (Application.isPlaying) return this.m_Section.Get(this.m_Args);

            GameObject sceneReference = this.m_Section.EditorValue;
            return sceneReference != null ? sceneReference : this.gameObject;
        }
    }
}