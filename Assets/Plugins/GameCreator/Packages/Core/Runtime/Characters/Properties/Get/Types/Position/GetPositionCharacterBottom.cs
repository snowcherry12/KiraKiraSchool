using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Character Position Feet")]
    [Category("Characters/Character Position Feet")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow, typeof(OverlayBar))]
    [Description("Returns the bottom (feet) position of the Character")]

    [Serializable]
    public class GetPositionCharacterBottom : PropertyTypeGetPosition
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public GetPositionCharacterBottom()
        { }

        public GetPositionCharacterBottom(Character character)
        {
            this.m_Character = GetGameObjectCharactersInstance.CreateWith(character);
        }

        public override Vector3 Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null ? character.Feet : default;
        }

        public static PropertyGetPosition Create => new PropertyGetPosition(
            new GetPositionCharacterBottom()
        );
        
        public static PropertyGetPosition CreateWith(Character character)
        {
            return new PropertyGetPosition(
                new GetPositionCharacterBottom(character)
            );
        }

        public override Vector3 EditorValue
        {
            get
            {
                GameObject gameObject = this.m_Character.EditorValue;
                if (gameObject == null) return default;

                Character character = gameObject.GetComponent<Character>();
                return character != null ? character.Feet : default;
            }
        }

        public override string String => $"{this.m_Character} Feet";
    }
}