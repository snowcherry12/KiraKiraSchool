using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Journal")]
    [Category("Quests/Journal")]
    
    [Image(typeof(IconJournalSolid), ColorTheme.Type.Yellow)]
    [Description("A Journal component attached to a scene Game Object or prefab")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectQuestsJournal : PropertyTypeGetGameObject
    {
        [SerializeField] protected Journal m_Journal;

        public override GameObject Get(Args args) => this.m_Journal != null 
            ? this.m_Journal.gameObject 
            : null;
        
        public override GameObject Get(GameObject gameObject) => this.m_Journal != null 
            ? this.m_Journal.gameObject 
            : null;
        
        public override T Get<T>(Args args)
        {
            if (typeof(T) == typeof(Journal)) return this.m_Journal as T;
            return base.Get<T>(args);
        }
        
        public GetGameObjectQuestsJournal() : base()
        { }

        public GetGameObjectQuestsJournal(GameObject journal) : this()
        {
            this.m_Journal = journal != null ? journal.Get<Journal>() : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectQuestsJournal instance = new GetGameObjectQuestsJournal();
            return new PropertyGetGameObject(instance);
        }

        public override GameObject EditorValue => m_Journal != null
            ? this.m_Journal.gameObject
            : null;

        public override string String => this.m_Journal != null
            ? this.m_Journal.gameObject.name
            : "(none)";
    }
}