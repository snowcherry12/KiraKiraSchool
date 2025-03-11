using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class ReloadItem : TPolymorphicItem<ReloadItem>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_SkipWhenFull;
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        
        [SerializeField] private Reload m_Asset;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => this.m_Asset != null ? this.m_Asset.name : "(none)";

        public Reload Asset => this.m_Asset;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Check(Character character, bool isFull)
        {
            if (this.m_SkipWhenFull && isFull) return false;
            return this.m_Conditions.Check(character.Args);
        }
    }
}