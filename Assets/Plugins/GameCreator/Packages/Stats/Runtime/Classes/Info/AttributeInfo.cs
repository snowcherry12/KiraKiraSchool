using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class AttributeInfo : TInfo
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public AttributeInfo() : base()
        {
            this.m_Acronym = new PropertyGetString("ATT");
            this.m_Name = new PropertyGetString("Attribute Name");
            this.m_Description = new PropertyGetString("Description...");
        }
    }
}