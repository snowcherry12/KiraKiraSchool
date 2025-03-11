using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Character Target")]
    [Description("Aims at the combat Target of a Character component or forward if none exists")]
    
    [Category("Character Target")]
    [Image(typeof(IconBullsEye), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class AimCharacterTarget : TAim
    {
        private const float INFINITY = 999f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Vector3 GetPoint(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return default;

            GameObject target = character.Combat.Targets.Primary;
            return target != null
                ? target.transform.position
                : character.transform.TransformPoint(Vector3.forward * INFINITY);
        }
    }
}