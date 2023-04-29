using KennethDevelops.ProLibrary.DataStructures;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Managers{
    
    public class ResourcesManager{
        
        private static LookUpTable<string, Object> _lookUpTable;

        public static T Load<T>(string url) where T : Object{
            if (_lookUpTable == null)
                _lookUpTable = new LookUpTable<string, Object>(fileurl => Resources.Load(fileurl));
            return (T)_lookUpTable.GetValue(url);
        }
        
    }
    
}