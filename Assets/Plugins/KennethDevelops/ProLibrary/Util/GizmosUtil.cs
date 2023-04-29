using UnityEngine;

namespace KennethDevelops.ProLibrary.Util{
    public static class GizmosUtil {
    
        public static void DrawArrow(Vector3 point, float bodyLength, float headLength, Vector3 forward, Vector3 right){
            var arrowHeadPoint  = point + forward * bodyLength;
            var arrowLeftPoint  = point + forward * (bodyLength - headLength) + -right * headLength;
            var arrowRightPoint = point + forward * (bodyLength - headLength) + right * headLength;

            Gizmos.DrawLine(point, arrowHeadPoint);
            Gizmos.DrawLine(arrowHeadPoint, arrowLeftPoint);
            Gizmos.DrawLine(arrowHeadPoint, arrowRightPoint);
        }

    }
}
