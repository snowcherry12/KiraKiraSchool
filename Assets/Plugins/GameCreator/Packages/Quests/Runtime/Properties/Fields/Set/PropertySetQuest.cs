using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Quests
{
    [Serializable]
    public class PropertySetQuest : TPropertySet<PropertyTypeSetQuest, Quest>
    {
        public PropertySetQuest() : base(new SetQuestNone())
        { }

        public PropertySetQuest(PropertyTypeSetQuest defaultType) : base(defaultType)
        { }
    }
}