using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Moon Gravity")]
    [Category("Physics/Moon Gravity")]
    
    [Image(typeof(IconApple), ColorTheme.Type.Blue)]
    [Description("The gravity on the Moon in units per second square")]
    
    [Serializable]
    public class GetDecimalPhysicsGravityMoon : PropertyTypeGetDecimal
    {
        private const float GRAVITY = 1.62f;
        
        public override double Get(Args args) => GRAVITY;
        public override double Get(GameObject gameObject) => GRAVITY;
        
        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalPhysicsGravityMoon()
        );
        
        public override double EditorValue => GRAVITY;
        
        public override string String => "Moon Gravity";
    }
}