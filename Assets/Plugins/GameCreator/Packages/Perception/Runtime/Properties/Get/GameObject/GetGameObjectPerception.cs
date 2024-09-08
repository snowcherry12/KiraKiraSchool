using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Perception
{
    [Title("Perception")]
    [Category("Perception/Perception")]
    
    [Image(typeof(IconPerception), ColorTheme.Type.Purple)]
    [Description("Reference to a Perception game object")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectPerception : PropertyTypeGetGameObject
    {
        [SerializeField] private Perception m_Perception;

        public GetGameObjectPerception()
        { }

        public GetGameObjectPerception(Perception perception)
        {
            this.m_Perception = perception;
        }

        public override GameObject Get(Args args)
        {
            return this.m_Perception != null ? this.m_Perception.gameObject : null;
        }

        public override GameObject Get(GameObject gameObject)
        {
            return this.m_Perception != null ? this.m_Perception.gameObject : null;
        }

        public static PropertyGetGameObject Create => new PropertyGetGameObject(
            new GetGameObjectPerception()
        );

        public static PropertyGetGameObject CreateWith(Perception perception)
        {
            return new PropertyGetGameObject(
                new GetGameObjectPerception(perception)
            );
        }

        public override string String => this.m_Perception != null
            ? this.m_Perception.gameObject.name
            : "(none)";

        public override GameObject EditorValue => this.m_Perception != null
            ? this.m_Perception.gameObject 
            : null;
    }
}