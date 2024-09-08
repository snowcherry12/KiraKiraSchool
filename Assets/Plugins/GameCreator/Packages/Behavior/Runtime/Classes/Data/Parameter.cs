using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class Parameter : TVariable
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private string m_Name;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string Name => this.m_Name;

        public override string Title => $"{this.m_Name}: {this.m_Value}";

        public override TVariable Copy => new Parameter
        {
            m_Name = this.m_Name,
            m_Value = this.m_Value.Copy
        };

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Parameter() : base()
        { }
        
        public Parameter(IdString typeID) : base(typeID)
        { }

        public Parameter(string name, TValue value) : this()
        {
            this.m_Name = name;
            this.m_Value = value;
        }
    }
}