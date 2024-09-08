using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Dialogue Played")]
    [Description("Returns true if the Dialogue component has been played")]

    [Category("Dialogue/Dialogue Played")]
    
    [Parameter("Dialogue", "The Dialogue component")]

    [Keywords("Dialogue", "Text", "Line", "Choice")]
    [Image(typeof(IconNodeText), ColorTheme.Type.Green)]
    
    [Serializable]
    public class ConditionDialogueHasPlayed : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Dialogue = GetGameObjectDialogue.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"has {this.m_Dialogue} played";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Dialogue dialogue = this.m_Dialogue.Get<Dialogue>(args);
            return dialogue != null && dialogue.Story.Visits.IsVisited;
        }
    }
}