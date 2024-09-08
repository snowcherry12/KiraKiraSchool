using System;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    internal class Domain
    {
        [NonSerialized] private Table m_Table;
        
        [NonSerialized] private GameObject m_Source;
        [NonSerialized] private GameObject m_Target;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Table Table
        {
            get => this.m_Table != null ? this.m_Table : throw new Exception("Table not found");
            private set => this.m_Table = value;
        }
        
        public GameObject Source
        {
            get => this.m_Source != null ? this.m_Source : throw new Exception("Source not found");
            private set => this.m_Source = value;
        }
        
        public GameObject Target
        {
            get => this.m_Target != null ? this.m_Target : throw new Exception("Target not found");
            private set => this.m_Target = value;
        }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Domain()
        {
            this.Table = null;
            this.Source = null;
            this.Target = null;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Set(Table table, GameObject source, GameObject target)
        {
            this.Table = table;
            this.Source = source;
            this.Target = target;
        }
    }
}