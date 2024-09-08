using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public struct Connection: IEquatable<Connection>
    {
        public static readonly Connection NONE = new Connection(string.Empty);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private string m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string Value => this.m_Value;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Connection(string value)
        {
            this.m_Value = value;
        }

        // OVERRIDES: -----------------------------------------------------------------------------
        
        public override int GetHashCode() => this.m_Value.GetHashCode();

        public bool Equals(Connection other)
        {
            return this.m_Value == other.m_Value;
        }

        public override bool Equals(object other)
        {
            return other is Connection otherConnection && this.Equals(otherConnection);
        }

        public override string ToString()
        {
            return this.m_Value;
        }
        
        // OPERATORS: -----------------------------------------------------------------------------
        
        public static bool operator ==(Connection left, Connection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Connection left, Connection right)
        {
            return !left.Equals(right);
        }
    }
}