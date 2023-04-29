namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class IntExtensions{
        
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
        public static bool IsInRangeInclusive(this int value, int from, int to){
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
        public static bool IsInRangeExclusive(this int value, int from, int to){
            return value > from && value < to;
        }
        
    }
    
}