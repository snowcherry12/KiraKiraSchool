using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetQuestNone : PropertyTypeSetQuest
    {
        public override void Set(Quest value, Args args)
        { }
        
        public override void Set(Quest value, GameObject gameObject)
        { }

        public static PropertySetQuest Create => new PropertySetQuest(
            new SetQuestNone()
        );

        public override string String => "(none)";
    }
}