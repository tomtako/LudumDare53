using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class RectExtensions{
        
        /// <summary>
        /// Returns the position (Vector2) of the Rect's center.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Vector2 GetCenter(this Rect rect){
            return new Vector2(rect.position.x + rect.width / 2, rect.position.y + rect.height / 2);
        }

        /// <summary>
        /// Adds a x value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Rect AddX(this Rect rect, float x){
            return rect.SetX(rect.x + x);
        }

        /// <summary>
        /// Adds a y value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Rect AddY(this Rect rect, float y){
            return rect.SetY(rect.y + y);
        }

        /// <summary>
        /// Adds a width value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Rect AddWith(this Rect rect, float width){
            return rect.SetWidth(rect.width + width);
        }

        /// <summary>
        /// Adds a height value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rect AddHeight(this Rect rect, float height){
            return rect.SetHeight(rect.height + height);
        }
        
        /// <summary>
        /// Sets the x value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Rect SetX(this Rect rect, float x){
            return new Rect(x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        /// Sets the y value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Rect SetY(this Rect rect, float y){
            return new Rect(rect.x, y, rect.width, rect.height);
        }

        /// <summary>
        /// Sets the width value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Rect SetWidth(this Rect rect, float width){
            return new Rect(rect.x, rect.y, width, rect.height);
        }

        /// <summary>
        /// Sets the height value to the current Rect.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rect SetHeight(this Rect rect, float height){
            return new Rect(rect.x, rect.y, rect.width, height);
        }
        
    }
}