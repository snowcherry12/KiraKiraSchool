using System.Text;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public static class TransformUtils
    {
        // POINT: ---------------------------------------------------------------------------------
        
        /// <summary>
        /// Converts a point from local to world space, using the position, rotation and scale
        /// values, instead of a Transform reference.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Vector3 TransformPoint(
            Vector3 point,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            Matrix4x4 localToWorldMatrix = Matrix4x4.TRS(position, rotation, scale);
            return localToWorldMatrix.MultiplyPoint3x4(point);
        }
        
        /// <summary>
        /// Converts a point from world space to world space, using the position, rotation and
        /// scale values, instead of the Transform reference.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Vector3 InverseTransformPoint(
            Vector3 point,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            Matrix4x4 localToWorldMatrix = Matrix4x4.TRS(position, rotation, scale);
            return localToWorldMatrix.inverse.MultiplyPoint3x4(point);
        }
        
        // ROTATIONS: -----------------------------------------------------------------------------
        
        /// <summary>
        /// Converts a rotation from local to world space, using the position, rotation and scale
        /// values, instead of a Transform reference.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Quaternion TransformRotation(Quaternion value, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4x4 transformationMatrix = Matrix4x4.TRS(position, rotation, scale);
            Vector3 transformedDirection = transformationMatrix.MultiplyVector(value * Vector3.forward);  
            Vector3 transformedUp = transformationMatrix.MultiplyVector(value * Vector3.up);
            
            return Quaternion.LookRotation(transformedDirection, transformedUp); 
        }

        
        /// <summary>
        /// Converts a rotation from world space to local space, using the position, rotation and
        /// scale values, instead of the Transform reference.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Quaternion InverseTransformRotation(
            Quaternion value,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            Matrix4x4 transformationMatrix = Matrix4x4.TRS(position, rotation, scale);
            Matrix4x4 inverseMatrix = Matrix4x4.Inverse(transformationMatrix);
            Vector3 transformedDirection = inverseMatrix.MultiplyVector(value * Vector3.forward);  
            Vector3 transformedUp = inverseMatrix.MultiplyVector(value * Vector3.up); 
            
            return Quaternion.LookRotation(transformedDirection, transformedUp);
        }
        
        // PATHS: ---------------------------------------------------------------------------------
        
        /// <summary>
        /// Returns the path from the top-most ancestor (or parent if specified) up until the
        /// designated transform target
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static string GetHierarchyPath(Transform transform, Transform parent = null)
        {
            if (transform == null) return string.Empty;
            StringBuilder path = new StringBuilder(transform.gameObject.name);

            while (transform.parent != null && transform.parent != parent)
            {
                transform = transform.parent;
                path.Insert(0, $"{transform.gameObject.name}/");
            }

            return path.ToString();
        }
    }
}