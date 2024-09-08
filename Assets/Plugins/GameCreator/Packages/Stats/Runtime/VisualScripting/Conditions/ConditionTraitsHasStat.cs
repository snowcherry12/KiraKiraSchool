using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Traits has Stat")]
    [Category("Stats/Traits has Stat")]
    
    [Image(typeof(IconTraits), ColorTheme.Type.Pink)]
    [Description("Returns true if the targeted Traits component has the specified Stat")]

    [Parameter("Traits", "The targeted game object with a Traits component")]
    [Parameter("Stat", "The Stat asset")]

    [Serializable]
    public class ConditionTraitsHasStat : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => $"do {this.m_Traits} have {this.m_Stat}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Traits target = this.m_Traits.Get<Traits>(args);
            if (target == null) return false;

            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return false;
            
            for (int i = 0; i < target.Class.StatsLength; ++i)
            {
                StatItem candidate = target.Class.GetStat(i);
                if (candidate.Stat == stat) return true;
            }

            return false;
        }
    }
}
