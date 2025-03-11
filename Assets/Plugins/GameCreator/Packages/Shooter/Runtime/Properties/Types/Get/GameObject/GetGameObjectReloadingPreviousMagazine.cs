using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Reloading Previous Magazine")]
    [Category("Shooter/Reloading Previous Magazine")]
    
    [Image(typeof(IconMagazine), ColorTheme.Type.Yellow)]
    [Description("The game object reference of the empty Magazine reloaded by a Character")]

    [Serializable]
    public class GetGameObjectReloadingPreviousMagazine : PropertyTypeGetGameObject
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        public override GameObject Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null
                ? character.Combat.RequestStance<ShooterStance>().Reloading.PreviousMagazine
                : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectReloadingPreviousMagazine instance = new GetGameObjectReloadingPreviousMagazine();
            return new PropertyGetGameObject(instance);
        }

        public override string String => $"{this.m_Character} previous Magazine";
    }
}