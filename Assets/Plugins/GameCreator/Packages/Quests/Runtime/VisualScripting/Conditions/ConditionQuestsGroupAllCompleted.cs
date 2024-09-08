using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Are all Quests Completed")]
    [Description("Returns true if at least one Quest from a List is Complete")]

    [Category("Quests/Groups/Are all Quests Completed")]

    [Keywords("Journal", "Mission", "Group")]
    
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Green, typeof(OverlayListVariable))]
    [Serializable]
    public class ConditionQuestsGroupAllCompleted : Condition
    {
        [SerializeField] private PropertyGetGameObject m_Journal = GetGameObjectPlayer.Create();
        [SerializeField] private CollectorListVariable m_Quests = new CollectorListVariable();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"are all from {this.m_Quests} Completed";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Journal journal = this.m_Journal.Get<Journal>(args);
            if (journal == null) return false;

            List<object> list = this.m_Quests.Get(args);
            if (list.Count == 0) return false;
            
            foreach (object item in list)
            {
                if (item is not Quest quest) return false;
                if (!journal.IsQuestCompleted(quest)) return false;
            }

            return true;
        }
    }
}
