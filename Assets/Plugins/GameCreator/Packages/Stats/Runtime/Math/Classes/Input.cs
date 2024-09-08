using System;
using System.Text.RegularExpressions;

namespace GameCreator.Runtime.Stats
{
    internal delegate double InputHandle(Domain domain, string name);

    internal class Input
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Regex m_Regex;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field:NonSerialized] public InputHandle Function { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Input(string pattern, InputHandle function)
        {
            this.m_Regex = new Regex(pattern);
            this.Function = function;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Match Match(string input)
        {
            return this.m_Regex.Match(input);
        }
    }
}