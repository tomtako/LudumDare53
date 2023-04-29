using System;
using System.Collections.Generic;
using UnityEngine;

namespace KennethDevelops.ProLibrary.DataStructures.Pool{

    internal class ObjectPool<T> {

        private Func<T>    _create;
        private Func<T, T> _disable;

        private List<T> _uninstantiated;


        public ObjectPool(Func<T> createFunction, Func<T, T> disableFunction, int initialQuantity) {
            _create  = createFunction;
            _disable = disableFunction;

            _uninstantiated = new List<T>();

            for (var i = 0; i < initialQuantity; i++) {
                var obj = _create();
                obj = _disable(obj);
                _uninstantiated.Add(obj);
            }
        }

        public T AcquireObject(){
            if (_uninstantiated.Count == 0) {
                var instance = _create();
                return instance;
            }
            
            var obj = _uninstantiated[_uninstantiated.Count - 1];
            
            if (obj.Equals(default(T))){
                Debug.LogWarning("[ProLibrary - PoolManager] An object was illegaly removed from the pool. However, we can continue working ;)");
                _uninstantiated.RemoveAt(_uninstantiated.Count - 1);
                return AcquireObject();
            }
            
            _uninstantiated.Remove(obj);
            return obj;
        }

        public void ReleaseObject(T obj){
            obj = _disable(obj);
            _uninstantiated.Add(obj);
        }

    }
    
}