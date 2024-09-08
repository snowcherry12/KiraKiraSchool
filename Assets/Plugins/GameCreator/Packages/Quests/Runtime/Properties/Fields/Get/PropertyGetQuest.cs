using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class PropertyGetQuest : TPropertyGet<PropertyTypeGetQuest, Quest>
    {
        public PropertyGetQuest() : base(new GetQuestInstance())
        { }

        public PropertyGetQuest(PropertyTypeGetQuest defaultType) : base(defaultType)
        { }
    }
}