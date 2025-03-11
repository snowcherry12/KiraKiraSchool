using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Reloading New Magazine")]
    [Category("Shooter/Reloading New Magazine")]
    
    [Image(typeof(IconMagazine), ColorTheme.Type.Green)]
    [Description("The game object reference of the new Magazine reloaded by a Character")]

    [Serializable]
    public class GetGameObjectReloadingCurrentMagazine : PropertyTypeGetGameObject
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();

        public override GameObject Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null
                ? character.Combat.RequestStance<ShooterStance>().Reloading.CurrentMagazine
                : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectReloadingCurrentMagazine instance = new GetGameObjectReloadingCurrentMagazine();
            return new PropertyGetGameObject(instance);
        }

        public override string String => $"{this.m_Character} new Magazine";
    }
}