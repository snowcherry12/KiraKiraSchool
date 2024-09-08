using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Character Target")]
    [Category("Melee/Character Target")]
    
    [Image(typeof(IconBullsEye), ColorTheme.Type.Yellow)]
    [Description("A midpoint Vector3 offset between the chosen character and its combat target")]

    [Serializable]
    public class GetDirectionMeleeCharacterTarget : PropertyTypeGetDirection
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] [Range(0f, 1f)] private float m_Ratio = 0.5f;

        public override Vector3 Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return Vector3.zero;

            Vector3 currentPosition = character.transform.position;
            Vector3 targetPosition = character.Combat.Targets.Primary != null
                ? character.Combat.Targets.Primary.transform.position
                : currentPosition;

            return Vector3.Lerp(currentPosition, targetPosition, this.m_Ratio) - currentPosition;
        }

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionMeleeCharacterTarget()
        );

        public override string String => $"{this.m_Character} Target";
    }
}
