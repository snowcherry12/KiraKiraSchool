using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public static class TwoBoneSolver
    {
        private const float SQRT_EPSILON = 1e-8f;
        private const float ERROR_MARGIN = 0.001f;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Solves a two-bone IK constraint where a, b and c are Transforms from root to tip
        /// </summary>
        /// <param name="data"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetRotation"></param>
        /// <param name="hint"></param>
        /// <param name="hintWeight"></param>
        public static TwoBoneData Run(
            TwoBoneData data,
            Vector3 targetPosition,
            Quaternion targetRotation,
            Transform hint,
            float hintWeight
        )
        {
            Vector3 aPosition = data.RootPosition;
            Vector3 bPosition = data.BodyPosition;
            Vector3 cPosition = data.HeadPosition;

            bool hasHint = hint != null && hintWeight > 0f;

            Vector3 ab = bPosition - aPosition;
            Vector3 bc = cPosition - bPosition;
            Vector3 ac = cPosition - aPosition;
            Vector3 at = targetPosition - aPosition;

            float abLength = ab.magnitude;
            float bcLength = bc.magnitude;
            float acLength = ac.magnitude;
            float atLength = at.magnitude;

            float prevAbcAngle = TriangleAngle(acLength, abLength, bcLength);
            float nextAbcAngle = TriangleAngle(atLength, abLength, bcLength);
            
            Vector3 axis = Vector3.Cross(ab, bc);
            if (axis.sqrMagnitude < SQRT_EPSILON)
            {
                axis = hasHint
                    ? Vector3.Cross(hint.position - aPosition, bc)
                    : Vector3.zero;

                if (axis.sqrMagnitude < SQRT_EPSILON)
                    axis = Vector3.Cross(at, bc);

                if (axis.sqrMagnitude < SQRT_EPSILON)
                    axis = Vector3.up;
            }

            axis = Vector3.Normalize(axis);

            float angle = 0.5f * (prevAbcAngle - nextAbcAngle);
            float sinAngle = Mathf.Sin(angle);
            float cosAngle = Mathf.Cos(angle);
            Quaternion deltaRotation = new Quaternion(
                axis.x * sinAngle,
                axis.y * sinAngle,
                axis.z * sinAngle,
                cosAngle
            );
            
            data.BodyRotation = deltaRotation * data.BodyRotation;

            cPosition = data.HeadPosition;
            ac = cPosition - aPosition;
            data.RootRotation = Quaternion.FromToRotation(ac, at) * data.RootRotation;

            if (hasHint)
            {
                float acSqrMag = ac.sqrMagnitude;
                if (acSqrMag > 0f)
                {
                    bPosition = data.BodyPosition;
                    cPosition = data.HeadPosition;
                    ab = bPosition - aPosition;
                    ac = cPosition - aPosition;

                    Vector3 acNormal = ac / Mathf.Sqrt(acSqrMag);
                    Vector3 ah = hint.position - aPosition;
                    Vector3 abProjection = ab - acNormal * Vector3.Dot(ab, acNormal);
                    Vector3 ahProjection = ah - acNormal * Vector3.Dot(ah, acNormal);

                    float maxReach = abLength + bcLength;
                    if (abProjection.sqrMagnitude > maxReach * maxReach * ERROR_MARGIN &&
                        ahProjection.sqrMagnitude > 0f)
                    {
                        Quaternion hintRotation = Quaternion.FromToRotation(
                            abProjection,
                            ahProjection
                        );
                        
                        hintRotation.x *= hintWeight;
                        hintRotation.y *= hintWeight;
                        hintRotation.z *= hintWeight;
                        hintRotation = Quaternion.Normalize(hintRotation);
                        data.RootRotation = hintRotation * data.RootRotation;
                    }
                }
            }

            data.HeadRotation = targetRotation;
            return data;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static float TriangleAngle(float aLen, float aLen1, float aLen2)
        {
            float c = Mathf.Clamp(
                (aLen1 * aLen1 + aLen2 * aLen2 - aLen * aLen) / (aLen1 * aLen2) / 2.0f,
                -1.0f,
                +1.0f
            );
            
            return Mathf.Acos(c);
        }
    }
}