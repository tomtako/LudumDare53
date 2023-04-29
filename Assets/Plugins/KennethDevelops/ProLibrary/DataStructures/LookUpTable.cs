using System;
using System.Collections.Generic;

namespace KennethDevelops.ProLibrary.DataStructures{
    
    public class LookUpTable<TKey, TValue> {

        private Dictionary<TKey, TValue> _dictionary;
        private Func<TKey, TValue>       _process;
        

        public LookUpTable(Func<TKey, TValue> process){
            _dictionary = new Dictionary<TKey, TValue>();
            _process    = process;
        }

        public TValue GetValue(TKey key){
            if (!_dictionary.ContainsKey(key)) _dictionary.Add(key,_process(key));
            return _dictionary[key];
        }

        public TValue this[TKey key]{
            get{ return GetValue(key); }
        } 

    }
    
}