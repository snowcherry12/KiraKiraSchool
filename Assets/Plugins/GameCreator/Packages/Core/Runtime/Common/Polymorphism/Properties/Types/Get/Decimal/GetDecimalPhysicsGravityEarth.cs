using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Earth Gravity")]
    [Category("Physics/Earth Gravity")]
    
    [Image(typeof(IconApple), ColorTheme.Type.Green)]
    [Description("The gravity in planet Earth in units per second square")]
    
    [Serializable]
    public class GetDecimalPhysicsGravityEarth : PropertyTypeGetDecimal
    {
        private const float GRAVITY = 9.81f;
        
        public override double Get(Args args) => GRAVITY;
        public override double Get(GameObject gameObject) => GRAVITY;
        
        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalPhysicsGravityEarth()
        );

        public override double EditorValue => GRAVITY;

        public override string String => "Gravity";
    }
}