using System.Collections.Generic;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class TransformExtensions{
        
        /* TODO: Improve
        public static float SqrDistance(this Transform transform, Transform other){
            return transform.position.SqrDistance(other.position);
        }

        public static float SqrDistance(this Transform transform, Vector3 other){
            return transform.position.SqrDistance(other);
        }

        public static float SqrDistance2D(this Transform transform, Transform other){
            return transform.position.SetY(0).SqrDistance(other.position.SetY(0));
        }

        public static float SqrDistance2D(this Transform transform, Vector3 other){
            return transform.position.SetY(0).SqrDistance(other.SetY(0));
        }
        */

        /// <summary>
        /// Returns the direct childs of the Transform instance, excluding grandchildren, great-grand children and so on.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform[] GetDirectChildren(this Transform transform){
            var children = new Transform[transform.childCount];

            for(var i = 0; i < transform.childCount; i++)
                children[i] = transform.GetChild(i);

            return children;
        }

        /// <summary>
        /// Returns the direct children's components of the Transform instance, excluding grandchildren,
        /// great-grand children and so on.
        /// </summary>
        /// <param name="transform"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetComponentsInDirectChildren<T>(this Transform transform) where T : Component{
            var components = new List<T>();

            for(var i = 0; i < transform.childCount; i++) {
                var component = transform.GetChild(i).GetComponent<T>();
                if (component != null) components.Add(component);
            }

            return components.ToArray();
        }

        /// <summary>
        /// Returns the direct children's components of the GameObject instance, excluding grandchildren,
        /// great-grand children and so on.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetComponentsInDirectChildren<T>(this GameObject gameObject) where T : Component{
            return gameObject.transform.GetComponentsInDirectChildren<T>();
        }
        
    }
    
}