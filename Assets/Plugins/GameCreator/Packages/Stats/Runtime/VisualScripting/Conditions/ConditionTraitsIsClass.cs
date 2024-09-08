using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Is Traits of Class")]
    [Category("Stats/Is Traits of Class")]
    
    [Image(typeof(IconTraits), ColorTheme.Type.Pink)]
    [Description("Returns true if the targeted Traits component has the specified Class")]

    [Parameter("Traits", "The targeted game object with a Traits component")]
    [Parameter("Class", "The Class asset")]

    [Serializable]
    public class ConditionTraitsIsClass : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private Class m_Class;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => string.Format(
            "is {0} of {1}",
            this.m_Traits,
            this.m_Class != null 
                ? this.m_Class.name 
                : "(none)"
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Traits target = this.m_Traits.Get<Traits>(args);
            if (target == null) return false;
            
            return this.m_Class != null && target.Class == this.m_Class;
        }
    }
}
