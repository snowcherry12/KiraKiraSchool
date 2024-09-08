using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    
    [Serializable]
    public class StatItem : TPolymorphicItem<StatItem>
    {
        [SerializeField] private bool m_IsHidden;
        [SerializeField] private Stat m_Stat;

        [SerializeField] private EnablerDouble m_ChangeBase = new EnablerDouble(false, 100);
        [SerializeField] private EnablerFormula m_ChangeFormula = new EnablerFormula(false, null);

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsHidden => m_IsHidden;
        public Stat Stat => this.m_Stat;

        public override string Title => this.m_Stat != null 
            ? TextUtils.Humanize(this.m_Stat.ID.String) 
            : "(none)";

        public double Base
        {
            get
            {
                if (this.m_Stat == null) return 0f;
                return this.m_ChangeBase.IsEnabled
                    ? this.m_ChangeBase.Value
                    : this.m_Stat.Value;
            }
        }

        public Formula Formula
        {
            get
            {
                if (this.m_Stat == null) return null;
                return this.m_ChangeFormula.IsEnabled
                    ? this.m_ChangeFormula.Value
                    : this.m_Stat.Formula;
            }
        }
    }
}