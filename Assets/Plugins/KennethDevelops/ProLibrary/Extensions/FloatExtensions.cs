using System;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class FloatExtensions{
        
        /// <summary>
        /// Returns if the value is in the defined range of values, including @from and @to
        /// </summary>
        /// <param name="value">The value we are checking whether or not it is in the range</param>
        /// 
        /// <param name="from">The minimum value of the defined range (included in range)</param>
        /// 
        /// <param name="to">The maximum value of the defined range (included in range)</param>
        /// 
        /// <returns>True if it is on the range, False if it is not</returns>
        public static bool IsInRangeInclusive(this float value, float from, float to){
            return value >= from && value <= to;
        }

        /// <summary>
        /// Returns if the value is in the defined range of values, excluding @from and @to
        /// </summary>
        /// <param name="value">The value we are checking whether or not it is in the range</param>
        /// 
        /// <param name="from">The minimum value of the defined range (excluded from range)</param>
        /// 
        /// <param name="to">The maximum value of the defined range (excluded from range)</param>
        /// 
        /// <returns>True if it is on the range, False if it is not</returns>
        public static bool IsInRangeExclusive(this float value, float from, float to){
            return value > from && value < to;
        }

        /// <summary>
        /// Clamps a value between a minimum and a maximum angle value (in degrees)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampAngle(this float angle, float min, float max){
            return Mathf.Clamp(NormalizeAngle(angle), min, max);
        }

        /// <summary>
        /// Normalizes and angle value (in degrees), making it be between the value ranges of 0 and 360.
        /// This means that the value 365 will be changed to 5, and the value -5 will be changed to 355
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float NormalizeAngle(this float angle){
            angle = angle % 360;
            if (angle < -360F) angle += 360F;
            if (angle > 360F) angle  -= 360F;
            return angle;
        }

        /// <summary>
        /// Converts an angle in degrees to radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Deg2Rad(this float angle){
            return angle * Mathf.PI / 180;
        }

        [Obsolete("Use Deg2Rad() instead")]
        public static float DegreesToRadians(this float angle){
            return angle.Deg2Rad();
        }

        /// <summary>
        /// Converts an angle in radians to degrees
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Rad2Deg(this float angle){
            return angle * 180 / Mathf.PI;
        }
        
        [Obsolete("Use Rad2Deg() instead")]
        public static float RadiansToDegrees(this float angle){
            return angle.Rad2Deg();
        } 

        /// <summary>
        /// Returns the value raised to power @exponent.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Pow(this float number, float exponent){
            return Mathf.Pow(number, exponent);
        }

        /* TODO: Change to .NET Framework 4.7
        #region Time
        public static string SecondsToMMSSFF(this float seconds){
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"mm\:ss\.ff");
        }

        public static string GetMinutes(this float seconds){
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"mm");
        }

        public static string GetSeconds(this float seconds){
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"ss");
        }
        #endregion
        */
    }
    
}