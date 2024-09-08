using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Awareness")]
    [Category("Perception/Awareness")]
    
    [Description("The awareness value of a game object from a Perception component")]
    [Image(typeof(IconAwareness), ColorTheme.Type.Blue)]

    [Serializable]
    public class GetDecimalAwareness : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return 0f;
            
            GameObject target = this.m_Target.Get(args);
            if (target == null) return 0f;
            
            Tracker tracker = perception.GetTracker(target);
            return tracker?.Awareness ?? 0f;
        }
        
        public override string String => $"{this.m_Perception}[{this.m_Target}] Awareness";
    }
}