using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Traits has Attribute")]
    [Category("Stats/Traits has Attribute")]
    
    [Image(typeof(IconTraits), ColorTheme.Type.Pink)]
    [Description("Returns true if the targeted Traits component has the specified Attribute")]

    [Parameter("Traits", "The targeted game object with a Traits component")]
    [Parameter("Attribute", "The Attribute asset")]

    [Serializable]
    public class ConditionTraitsHasAttribute : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => $"do {this.m_Traits} have {this.m_Attribute}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Traits target = this.m_Traits.Get<Traits>(args);
            if (target == null) return false;

            Attribute attribute = this.m_Attribute.Get(args);
            if (attribute == null) return false;

            for (int i = 0; i < target.Class.AttributesLength; ++i)
            {
                AttributeItem candidate = target.Class.GetAttribute(i);
                if (candidate.Attribute == attribute) return true;
            }

            return false;
        }
    }
}
