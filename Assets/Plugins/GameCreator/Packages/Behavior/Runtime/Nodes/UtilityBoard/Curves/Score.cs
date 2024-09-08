using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class Score : ISerializationCallbackReceiver
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_Value = GetDecimalParameter.Create();
        
        [SerializeField] private AnimationCurve m_Curve = new AnimationCurve
        {
            keys = new[]
            {
                new Keyframe
                {
                    time = 0f,
                    value = 1f,
                    inTangent = 0f,
                    outTangent = -2.5f,
                    inWeight = 0f,
                    outWeight = 0.1f
                },
                new Keyframe
                {
                    time = 1f,
                    value = 0.1f,
                    inTangent = -0f,
                    outTangent = 0f,
                    inWeight = 0.5f,
                    outWeight = 0f
                }
            }
        };
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float Evaluate(Args args, UtilityBoard graph)
        {
            if (graph == null) return 0f;
            
            float value = (float) this.m_Value.Get(args);
            float minimum = graph.GetMinimum(args);
            float maximum = graph.GetMaximum(args);

            value = Mathf.InverseLerp(minimum, maximum, value);
            
            float result = this.m_Curve.Evaluate(value);
            return Mathf.Lerp(minimum, maximum, result);
        }
        
        // SERIALIZATION: -------------------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            Keyframe[] keys = this.m_Curve.keys;
            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i].time = Math.Clamp(keys[i].time, 0f, 1f);
                keys[i].value = Math.Clamp(keys[i].value, 0f, 1f);
            }

            this.m_Curve.keys = keys;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            return this.m_Value.ToString();
        }
    }
}