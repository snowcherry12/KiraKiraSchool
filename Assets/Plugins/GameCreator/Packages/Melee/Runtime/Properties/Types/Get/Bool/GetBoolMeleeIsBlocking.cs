using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Is Blocking")]
    [Category("Melee/Is Blocking")]
    
    [Description("Returns true if the specified Character is blocking Melee attacks")]
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Green)]
    
    [Keywords("Melee", "Defend", "Block")]

    [Serializable] [HideLabelsInEditor]
    public class GetBoolMeleeIsBlocking : PropertyTypeGetBool
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public override bool Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null && character.Combat.Block.IsBlocking;
        }

        public static PropertyGetBool Create => new PropertyGetBool(
            new GetBoolMeleeIsBlocking()
        );
        
        public override string String => $"{this.m_Character} Blocking";
    }
}