using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Perception
{
    [Title("Evidence")]
    [Category("Perception/Evidence")]
    
    [Image(typeof(IconEvidence), ColorTheme.Type.Green)]
    [Description("Reference to a Evidence game object")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectEvidence : PropertyTypeGetGameObject
    {
        [SerializeField] private Evidence m_Evidence;

        public GetGameObjectEvidence()
        { }

        public GetGameObjectEvidence(Evidence evidence)
        {
            this.m_Evidence = evidence;
        }

        public override GameObject Get(Args args)
        {
            return this.m_Evidence != null ? this.m_Evidence.gameObject : null;
        }

        public override GameObject Get(GameObject gameObject)
        {
            return this.m_Evidence != null ? this.m_Evidence.gameObject : null;
        }

        public static PropertyGetGameObject Create => new PropertyGetGameObject(
            new GetGameObjectEvidence()
        );

        public static PropertyGetGameObject CreateWith(Evidence evidence)
        {
            return new PropertyGetGameObject(
                new GetGameObjectEvidence(evidence)
            );
        }

        public override string String => this.m_Evidence != null
            ? this.m_Evidence.gameObject.name
            : "(none)";

        public override GameObject EditorValue => this.m_Evidence != null
            ? this.m_Evidence.gameObject 
            : null;
    }
}