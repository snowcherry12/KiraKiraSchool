using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Local Input Direction")]
    [Category("Characters/Local Input Direction")]
    
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("The raw desired input direction of the Character in local space")]
    
    [Serializable]
    public class GetDirectionCharactersLocalInput : PropertyTypeGetDirection
    {
        [SerializeField]
        protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        
        public override Vector3 Get(Args args) => this.GetDirection(args);

        private Vector3 GetDirection(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null ? character.Player.LocalInputDirection : default;
        }
        
        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionCharactersLocalInput()
        );
        
        public override string String => $"{this.m_Character} Local Input";
    }
}