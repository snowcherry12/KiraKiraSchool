using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class StatInfo : TInfo
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StatInfo() : base()
        {
            this.m_Acronym = new PropertyGetString("STA");
            this.m_Name = new PropertyGetString("Stat Name");
            this.m_Description = new PropertyGetString("Description...");
        }
    }
}