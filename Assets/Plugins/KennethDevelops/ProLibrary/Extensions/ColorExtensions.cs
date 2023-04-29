using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class ColorExtension{
        
        /// <summary>
        /// Returns a new Color with an inverted color value.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Invert(this Color color){
            return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
        }

        /// <summary>
        /// Returns a new Color with a modified alpha value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color Alpha(this Color color, float alpha){
            return new Color(color.r, color.g, color.b, alpha);
        }
        
        /// <summary>
        /// Returns a new Color with a modified red value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="red"></param>
        /// <returns></returns>
        public static Color Red(this Color color, float red){
            return new Color(red, color.g, color.b, color.a);
        }
        
        /// <summary>
        /// Returns a new Color with a modified green value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="green"></param>
        /// <returns></returns>
        public static Color Green(this Color color, float green){
            return new Color(color.r, green, color.b, color.a);
        }
        
        /// <summary>
        /// Returns a new Color with a modified blue value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static Color Blue(this Color color, float blue){
            return new Color(color.r, color.g, blue, color.a);
        }

        /// <summary>
        /// Returns the grayscale version of the Color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToGrayscale(this Color color){
            return new Color(color.grayscale, color.grayscale, color.grayscale, 1f);
        }

        /// <summary>
        /// Tries to create a new Color with the prompted hex color value.
        /// If it fails it will return the value of originalColor.
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="originalColor"></param>
        /// <returns></returns>
        public static Color HtmlToColor(this string hex, Color originalColor = new Color()){
            var newColor = new Color();
            return ColorUtility.TryParseHtmlString(hex, out newColor) ? newColor : originalColor;
        }

        /// <summary>
        /// Tries to create a new Color with the prompted hex color value.
        /// If it fails it will return the value of originalColor.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HtmlToColor(this Color color, string hex){
            return hex.HtmlToColor(color);
        }
        
    }
    
}