using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Title("Thought")]
    [Category("Thought")]
    
    [Image(typeof(IconBelief), ColorTheme.Type.Green)]
    [Description("The boolean Thought initial value for Beliefs")]
    
    [Serializable]
    public class Thought : TPolymorphicItem<Thought>
    {
        [SerializeField] private IdString m_Name = new IdString("my-belief");
        [SerializeField] private PropertyGetBool m_Value = GetBoolTrue.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string Name => this.m_Name.String;

        public override string Title => $"{this.m_Name} = {this.m_Value}";

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool GetValue(Args args) => this.m_Value.Get(args);
    }
}