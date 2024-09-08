using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Parameter("Character", "The Character from which to calculate the direction")]

    [Keywords("Combat", "Melee", "Input", "Direction")]
    [Serializable]
    public abstract class TConditionMeleeInput : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected sealed override string Summary => $"is {this.m_Character} {this.Direction} Target";
        
        protected abstract string Direction { get; }
        protected abstract float DirectionSign { get; }

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Transform camera = ShortcutMainCamera.Transform;
            if (camera == null) return false;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            Vector3 playerDirection = character.Player.InputDirection;
            if (playerDirection == Vector3.zero) return false;

            GameObject target = character.Combat.Targets.Primary;
            if (target == null) return false;
            
            Vector3 direction = Vector3.Scale(
                target.transform.position - character.transform.position,
                Vector3Plane.NormalUp
            );
            
            Vector3 charactersDirection = camera.InverseTransformDirection(direction).XZ();
            Vector3 inputDirection = camera.InverseTransformDirection(playerDirection).XZ();

            return Vector2.Angle(charactersDirection * this.DirectionSign, inputDirection) <= 90f;
        }
    }
}
