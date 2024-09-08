using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Traits Color")]
    [Category("Stats/Traits Color")]
    
    [Image(typeof(IconTraits), ColorTheme.Type.Pink)]
    [Description("Returns the Color value of the Class assigned to a Traits component")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorTraits : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();

        public override Color Get(Args args)
        {
            Traits traits = this.m_Traits.Get<Traits>(args);
            return traits != null ? traits.Class.GetColor(args) : Color.black;
        }

        public override string String => $"{this.m_Traits} Color";
    }
}