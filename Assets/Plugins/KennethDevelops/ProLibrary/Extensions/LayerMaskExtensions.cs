using UnityEngine;

namespace KennethDevelops.ProLibrary.Extensions{
    
    public static class LayerMaskExtensions{
        
        /// <summary>
        /// Returns if the LayerMask includes the @layer value.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool IncludesLayer(this LayerMask layerMask, int layer){
            return ((1 << layer) & layerMask.value) != 0;
        }
        
    }
    
}