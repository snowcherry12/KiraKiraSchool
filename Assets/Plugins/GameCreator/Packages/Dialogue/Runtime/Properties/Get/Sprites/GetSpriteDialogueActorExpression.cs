using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Actor Expression")]
    [Category("Dialogue/Actor Expression")]
    
    [Image(typeof(IconExpression), ColorTheme.Type.Yellow)]
    [Description("The chosen expression of an Actor asset")]

    [Serializable] [HideLabelsInEditor]
    public class GetSpriteDialogueActorExpression : PropertyTypeGetSprite
    {
        [SerializeField] protected Expressing m_Expression = new Expressing();

        public override Sprite Get(Args args) => this.m_Expression.GetSprite(args);

        public static PropertyGetSprite Create()
        {
            GetSpriteDialogueActorExpression instance = new GetSpriteDialogueActorExpression();
            return new PropertyGetSprite(instance);
        }

        public override string String => this.m_Expression.ToString();
    }
}