using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Tag Visited")]
    [Description("Returns true if the Tag of a particular Dialogue has ran")]

    [Category("Dialogue/Tag Visited")]
    
    [Parameter("Dialogue", "The Dialogue component")]
    [Parameter("Tag", "The Tag name to check")]

    [Keywords("Dialogue", "Text", "Line", "Choice")]
    [Image(typeof(IconNodeText), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionDialogueTagVisited : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Dialogue = GetGameObjectDialogue.Create();
        [SerializeField] private IdString m_Tag = new IdString("");

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Tag} visited";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Dialogue dialogue = this.m_Dialogue.Get<Dialogue>(args);
            return dialogue != null && dialogue.Story.Visits.Tags.Contains(this.m_Tag);
        }
    }
}
