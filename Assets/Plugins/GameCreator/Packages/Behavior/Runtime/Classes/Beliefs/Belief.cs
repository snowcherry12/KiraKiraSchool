using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Belief")]
    [Category("Belief")]
    
    [Image(typeof(IconBelief), ColorTheme.Type.Blue)]
    [Description("The boolean Belief value")]
    
    [Serializable]
    public class Belief : TPolymorphicItem<Belief>
    {
        [SerializeField] private IdString m_Name;
        [SerializeField] private bool m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_Name} = {this.m_Value}";

        public IdString Name => this.m_Name;
        public bool Value => this.m_Value;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Belief()
        { }

        public Belief(string id, bool value) : this()
        {
            this.m_Name = new IdString(id);
            this.m_Value = value;
        }
    }
}