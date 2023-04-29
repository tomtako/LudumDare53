using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    public static class Vector3Extensions{

        public static Vector3 SetX(this Vector3 vector, float x){
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 SetY(this Vector3 vector, float y){
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 SetZ(this Vector3 vector, float z){
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector3 GetPerpendicularDirector(this Vector3 vector){
            var z = vector.x + vector.y / vector.z;
            return new Vector3(1, 1, z).normalized;
        }

        public static float SqrDistance(this Vector3 vector, Vector3 other){
            return (vector - other).sqrMagnitude;
        }

        /// <summary>
        /// Returns a new Vector2 using the given vector's x as x and the y as y
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 ToVector2XY(this Vector3 vector){
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Returns a new Vector2 using the given vector's x as x and the z as y
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 ToVector2XZ(this Vector3 vector){
            return new Vector2(vector.x, vector.z);
        }

        #region Direction

        /// <summary>
        /// Returns if current vector's direction is right
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool IsRight(this Vector3 direction, Vector3 right){
            var dotProduct = Vector3.Dot(right, direction.normalized);
            return dotProduct >= Mathf.Cos(45f.Deg2Rad());
        }

        /// <summary>
        /// Returns if current vector's direction is left
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool IsLeft(this Vector3 direction, Vector3 right){
            var dotProduct = Vector3.Dot(right, direction.normalized);
            return dotProduct <= Mathf.Cos(135f.Deg2Rad());
        }

        /// <summary>
        /// Returns if current vector's direction is either right or left
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool IsLateral(this Vector3 direction, Vector3 right){
            return direction.IsRight(right) || direction.IsLeft(right);
        }

        /// <summary>
        /// Returns if current vector's direction is forward 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static bool IsForward(this Vector3 direction, Vector3 forward){
            return direction.IsRight(forward); //same algorithm, only changing 'right' for 'forward'
        }

        /// <summary>
        /// Returns if current vector's direction is backward
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static bool IsBackward(this Vector3 direction, Vector3 forward){
            return direction.IsLeft(forward); //same algorithm, only changing 'right' for 'forward'
        }

        /// <summary>
        /// Returns if current vector's direction is either forward or backward
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static bool IsFrontal(this Vector3 direction, Vector3 forward){
            return direction.IsForward(forward) || direction.IsBackward(forward);
        }

        #endregion

    }
}