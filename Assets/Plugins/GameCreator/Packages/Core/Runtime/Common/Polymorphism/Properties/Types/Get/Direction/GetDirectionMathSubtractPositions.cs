using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Subtract Positions")]
    [Category("Math/Subtract Positions")]
    
    [Image(typeof(IconMinusCircle), ColorTheme.Type.Green)]
    [Description("Subtracts two positions to create a direction")]

    [Serializable]
    public class GetDirectionMathSubtractPositions : PropertyTypeGetDirection
    {
        [SerializeField] private PropertyGetDirection m_From = GetDirectionSelf.Create;
        [SerializeField] private PropertyGetDirection m_To = GetDirectionTarget.Create;

        public override Vector3 Get(Args args)
        {
            Vector3 position1 = this.m_From.Get(args);
            Vector3 position2 = this.m_To.Get(args);
            
            return position2 - position1;
        }

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionMathSubtractPositions()
        );

        public override string String => $"From {this.m_From} to {this.m_To})";
    }
}