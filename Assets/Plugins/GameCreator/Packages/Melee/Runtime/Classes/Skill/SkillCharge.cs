using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class SkillCharge
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private StateData m_State = new StateData(StateData.StateType.State);
        [SerializeField] private int m_Layer = Combat.DEFAULT_LAYER_CHARGE;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public StateData State => this.m_State;
        public int Layer => this.m_Layer;
    }
}