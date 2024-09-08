using System;
using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class MeleeMunition : TMunitionValue
    {
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override object Clone()
        {
            return new MeleeMunition();
        }
        
        // TO STRING: -----------------------------------------------------------------------------
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}