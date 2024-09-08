using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Are Quests Equal")]
    [Description("Returns true if two given Quest assets are the same")]

    [Category("Quests/Are Quests Equal")]

    [Keywords("Journal", "Mission", "Task")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Yellow)]
    [Serializable]
    public class ConditionQuestsCompareSame : Condition
    {
        [SerializeField] private PropertyGetQuest m_Quest1 = GetQuestInstance.Create();
        [SerializeField] private PropertyGetQuest m_Quest2 = GetQuestInstance.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Quest1} = {this.m_Quest2}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Quest quest1 = this.m_Quest1.Get(args);
            Quest quest2 = this.m_Quest2.Get(args);

            return quest1 != null && quest2 != null && quest1.Id.Hash == quest2.Id.Hash;
        }
    }
}
