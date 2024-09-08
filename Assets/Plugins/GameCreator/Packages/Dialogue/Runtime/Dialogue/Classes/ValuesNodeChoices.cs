using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class ValuesNodeChoices
    {
        private enum SingleChoice
        {
            ChooseAutomatically,
            PromptToChoose
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private bool m_HideUnavailable;
        [SerializeField] private bool m_HideVisited;
        [SerializeField] private bool m_SkipChoice;
        [SerializeField] private bool m_ShuffleChoices;

        [SerializeField] private TimedChoice m_TimedChoice = new TimedChoice();
        [SerializeField] private SingleChoice m_SingleChoice = SingleChoice.ChooseAutomatically;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool HideUnavailable => this.m_HideUnavailable;
        public bool HideVisited => this.m_HideVisited;
        public bool SkipChoice => this.m_SkipChoice;
        public bool ShuffleChoices => this.m_ShuffleChoices;
        
        public TimedChoice TimedChoice => this.m_TimedChoice;

        public bool AutoOneChoice => this.m_SingleChoice == SingleChoice.ChooseAutomatically;
    }
}