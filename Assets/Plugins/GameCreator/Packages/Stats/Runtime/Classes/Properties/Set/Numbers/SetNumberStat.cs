using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Base")]
    [Category("Stats/Stat Base")]
    
    [Description("Sets the base value of a Stat on a game object's Traits component")]
    [Image(typeof(IconStat), ColorTheme.Type.Red)]

    [Serializable]
    public class SetNumberStat : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        public override void Set(double value, Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return;

            traits.RuntimeStats.Get(stat.ID).Base = (float) value;
        }
        
        public override double Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return 0f;
            
            GameObject gameObject = this.m_Traits.Get(args);
            if (gameObject == null) return 0f;

            Traits traits = gameObject.Get<Traits>();
            return traits != null ? traits.RuntimeStats.Get(stat.ID).Value : 0f;
        }

        public static PropertySetNumber Create => new PropertySetNumber(
            new SetNumberStat()
        );
        
        public override string String => $"{this.m_Traits}[{this.m_Stat}].Base";
    }
}