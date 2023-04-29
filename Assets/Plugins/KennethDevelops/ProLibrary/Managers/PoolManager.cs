using System.Collections.Generic;
using System.Runtime.InteropServices;
using KennethDevelops.ProLibrary.DataStructures.Pool;
using KennethDevelops.ProLibrary.Util;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Managers
{

    public class PoolManager : MonoBehaviour
    {
        [Header("Whether or not this Pool is destroyed when a Scene is loaded")]
        public bool destroyOnLoad = true;

        [Space]
        [Header("Amount of Objects the Pool will start with")]
        public int initialQuantity = 5;
        public GameObject prefab;

        private ObjectPool<IPoolObject> _pool;
        private int _lastIndex;

        private readonly List<IPoolObject> _instantiated = new List<IPoolObject>();


        void Awake()
        {
            _pool = new ObjectPool<IPoolObject>(InstantiatePoolObject, DisposePoolObject, initialQuantity);

            if (!destroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public T AcquireObject<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour, IPoolObject
        {
            var poolObject = (T)_pool.AcquireObject();
            poolObject.gameObject.SetActive(true);

            poolObject.transform.position = position;
            poolObject.transform.rotation = rotation;

            poolObject.OnAcquire(this);

            _instantiated.Add(poolObject);

            return poolObject;
        }

        public T AcquireUiObject<T>(Vector2 anchoredPosition) where T : MonoBehaviour, IPoolObject
        {
            var poolObject = (T)_pool.AcquireObject();
            poolObject.gameObject.SetActive(true);

            ((RectTransform) poolObject.transform).anchoredPosition = anchoredPosition;

            poolObject.OnAcquire(this);

            _instantiated.Add(poolObject);

            return poolObject;
        }

        public void Dispose(IPoolObject poolObject)
        {
            _pool.ReleaseObject(poolObject);
            _instantiated.Remove(poolObject);
        }

        /// <summary>
        /// Disposes all instantiated IPoolObjects
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < _instantiated.Count; i++)
            {
                Dispose(_instantiated[i]);
            }
        }

        private IPoolObject InstantiatePoolObject()
        {
            var instance = Instantiate(prefab, transform, false);
            instance.gameObject.name = $"{prefab.name}[{_lastIndex}]";
            instance.gameObject.SetActive(false);
            _lastIndex++;
            return instance.GetComponent<IPoolObject>();
        }

        private IPoolObject DisposePoolObject(IPoolObject poolObject)
        {
            poolObject.OnDispose();

            var monoBehaviour = (MonoBehaviour) poolObject;
            monoBehaviour.gameObject.SetActive(false);

            return poolObject;
        }
    }
}