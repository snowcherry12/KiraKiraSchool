using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Content : TSerializableTree<Node>, ISerializationCallbackReceiver
    {
        #if UNITY_EDITOR
        
        public static DialogueSkin LAST_SKIN;
        
        #endif
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Role[] m_Roles = Array.Empty<Role>();
        
        [SerializeField] private DialogueSkin m_DialogueSkin;
        [SerializeField] private TimeMode m_Time = new TimeMode(TimeMode.UpdateMode.UnscaledTime);

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dictionary<Actor, PropertyGetGameObject> m_Troupe;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public DialogueSkin DialogueSkin
        {
            get => this.m_DialogueSkin;
            set => this.m_DialogueSkin = value;
        }

        public TimeMode Time => this.m_Time;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Content()
        {
            this.m_Troupe = new Dictionary<Actor, PropertyGetGameObject>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public int FindByTag(IdString tag)
        {
            foreach (KeyValuePair<int, TTreeDataItem<Node>> entry in this.m_Data)
            {
                IdString name = entry.Value.Value.Tag;
                if (name.Hash == tag.Hash) return entry.Key;
            }

            return NODE_INVALID;
        }
        
        public List<Tag> GetTags()
        {
            List<Tag> tags = new List<Tag>();
            foreach (KeyValuePair<int, TTreeDataItem<Node>> entry in this.m_Data)
            {
                IdString name = entry.Value.Value.Tag;
                if (name.Hash == IdString.EMPTY.Hash) continue;
                
                tags.Add(new Tag(name, entry.Key));
            }

            return tags;
        }
        
        public GameObject GetTargetFromActor(Actor actor, Args args)
        {
            if (actor == null) return null;
            return this.m_Troupe.TryGetValue(actor, out PropertyGetGameObject result) 
                ? result.Get(args) 
                : null;
        }

        #if UNITY_EDITOR
        
        public GameObject GetSceneReferenceFromActor(Actor actor)
        {
            if (actor == null) return null;
            return this.m_Troupe.TryGetValue(actor, out PropertyGetGameObject result) 
                ? result.EditorValue 
                : null;
        }
        
        #endif

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private Actor[] FindActors()
        {
            List<Actor> actors = new List<Actor>();
            
            foreach (KeyValuePair<int, TTreeDataItem<Node>> entry in this.m_Data)
            {
                Actor actor = entry.Value.Value.Actor;

                if (actor == null) continue;
                if (actors.Contains(actor)) continue;

                actors.Add(actor);
            }
            
            return actors.ToArray();
        }
        
        private bool ContainsActor(IEnumerable<Role> roles, Actor actor)
        {
            foreach (Role role in roles)
            {
                if (role.Actor == actor) return true;
            }

            return false;
        }
        
        private int SortRoles(Role a, Role b)
        {
            Actor aActor = a.Actor;
            Actor bActor = b.Actor;

            if (aActor == null) return 0;
            if (bActor == null) return 0;

            return string.Compare(
                aActor.name, bActor.name,
                StringComparison.InvariantCultureIgnoreCase
            );
        }
        
        // INTERNAL EDITOR METHODS: ---------------------------------------------------------------
        
        internal void EditorReset()
        {
            #if UNITY_EDITOR
            
            if (this.DialogueSkin != null) return;

            if (LAST_SKIN != null)
            {
                this.m_DialogueSkin = LAST_SKIN;
                return;
            }
            
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:DialogueSkin");
            if (guids.Length == 0) return;
        
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            DialogueSkin skin = UnityEditor.AssetDatabase.LoadAssetAtPath<DialogueSkin>(path);
        
            if (skin != null) this.m_DialogueSkin = skin;
            
            #endif
        }
        
        // SERIALIZATION METHODS: -----------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            
            Actor[] actors = this.FindActors();
            List<Role> newRoles = new List<Role>();

            foreach (Actor actor in actors)
            {
                if (actor == null) continue;
                if (ContainsActor(newRoles, actor)) continue;

                bool existing = false;
                foreach (Role role in this.m_Roles)
                {
                    if (role.Actor != actor) continue;
                    
                    newRoles.Add(role);
                    existing = true;
                    break;
                }
                
                if (existing) continue;
                newRoles.Add(new Role(actor));
            }

            newRoles.Sort(this.SortRoles);
            this.m_Roles = newRoles.ToArray();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.m_Troupe = new Dictionary<Actor, PropertyGetGameObject>();
            foreach (Role role in this.m_Roles)
            {
                if (role?.Actor == null) continue;
                this.m_Troupe.Add(role.Actor, role.Target);
            }
        }
    }
}