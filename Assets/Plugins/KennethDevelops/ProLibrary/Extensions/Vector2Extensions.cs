using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    public static class Vector2Extensions{
        
        public static Vector2 FaceDirection(this Vector2 vector, Vector2 direction){
            var tangent                = direction.y / direction.x;
            var angle                  = Mathf.Atan(tangent).Rad2Deg();
            if (direction.x < 0) angle += 180;

            return vector.RotateToAngle(angle.Deg2Rad());
        }

        public static Vector2 RotateToAngle(this Vector2 vector, float angle){
            return Quaternion.AngleAxis(angle.Rad2Deg(), new Vector3(0, 0, 1)) * vector;
        }
        
    }
}