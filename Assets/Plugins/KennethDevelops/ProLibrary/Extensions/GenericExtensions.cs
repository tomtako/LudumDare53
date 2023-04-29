using System.Linq;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class GenericExtensions{

        /// <summary>
        /// Returns true if an object is equal to any of the @values
        /// </summary>
        /// <param name="objToCompare"></param>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsIn<T>(this T objToCompare, params T[] values){
            return values.Any(n => n.Equals(objToCompare));
        }
        
    }
    
}