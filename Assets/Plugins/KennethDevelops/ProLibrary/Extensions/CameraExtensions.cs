using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class CameraExtensions{
        
        /// <summary>
        /// Returns whether or not the gameObject is visible to the camera.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool IsInFov(this Camera camera, GameObject gameObject){
            var viewportPoint = camera.WorldToViewportPoint(gameObject.transform.position);
            return viewportPoint.x.IsInRangeExclusive(0, 1) &&
                   viewportPoint.y.IsInRangeExclusive(0, 1) &&
                   viewportPoint.z > 0; //viewport.z has to be greater than 0, otherwise is facing backwards
        }
        
    }
    
}