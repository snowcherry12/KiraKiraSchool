using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class CompareQuestOrAny
    {
        private enum Option
        {
            Any = 0,
            Specific = 1
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Option m_Option = Option.Any;
        [SerializeField] private PropertyGetQuest m_Quest = GetQuestInstance.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool Any => this.m_Option == Option.Any;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public CompareQuestOrAny()
        { }

        public CompareQuestOrAny(PropertyGetQuest quest) : this()
        {
            this.m_Option = Option.Specific;
            this.m_Quest = quest;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(Quest compareTo, Args args)
        {
            if (this.Any) return true;
            if (compareTo == null) return false;
            
            Quest quest = this.Get(args);
            return quest != null && compareTo.Id.Hash == quest.Id.Hash;
        }
        
        public bool Match(Quest compareTo, GameObject args)
        {
            if (this.Any) return true;
            if (compareTo == null) return false;

            Quest quest = this.Get(args);
            return quest != null && compareTo.Id.Hash == quest.Id.Hash;
        }
        
        public Quest Get(Args args)
        {
            return this.m_Quest.Get(args);
        }

        public Quest Get(GameObject target)
        {
            return this.m_Quest.Get(target);
        }
    }
}
