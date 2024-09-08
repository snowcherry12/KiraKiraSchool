using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class StatusEffectInfo : TInfo
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StatusEffectInfo() : base()
        {
            this.m_Acronym = new PropertyGetString("SE");
            this.m_Name = new PropertyGetString("Status Effect Name");
            this.m_Description = new PropertyGetString("Description...");
        }
    }
}