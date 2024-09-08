using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class AttributeItem :  TPolymorphicItem<AttributeItem>
    {
        [SerializeField] private bool m_IsHidden;
        [SerializeField] private Attribute m_Attribute;
        
        [SerializeField] private EnablerRatio m_ChangeStartPercent = new EnablerRatio(false, 1f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsHidden => m_IsHidden;
        public Attribute Attribute => this.m_Attribute;

        public override string Title => this.m_Attribute != null 
            ? TextUtils.Humanize(this.m_Attribute.ID.String) 
            : "(none)";

        public double MinValue => this.m_Attribute != null ? this.m_Attribute.MinValue : 0f;
        public Stat MaxValue  => this.m_Attribute != null ? this.m_Attribute.MaxValue : null;

        public double StartPercent
        {
            get
            {
                if (this.m_Attribute == null) return 0f;
                return this.m_ChangeStartPercent.IsEnabled
                    ? this.m_ChangeStartPercent.Value
                    : this.m_Attribute.StartPercent;
            }
        }
    }
}