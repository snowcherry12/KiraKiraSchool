using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Mars Gravity")]
    [Category("Physics/Mars Gravity")]
    
    [Image(typeof(IconApple), ColorTheme.Type.Red)]
    [Description("The gravity on Mars in units per second square")]
    
    [Serializable]
    public class GetDecimalPhysicsGravityMars : PropertyTypeGetDecimal
    {
        private const float GRAVITY = 3.71f;
        
        public override double Get(Args args) => GRAVITY;
        public override double Get(GameObject gameObject) => GRAVITY;
        
        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalPhysicsGravityMars()
        );
        
        public override double EditorValue => GRAVITY;
        
        public override string String => "Mars Gravity";
    }
}