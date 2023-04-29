using System;
using KennethDevelops.ProLibrary.Managers;
using UnityEngine;

namespace KennethDevelops.ProLibrary.DataStructures.Pool{

    public interface IPoolObject{

        void OnAcquire(PoolManager poolManager);
        void OnDispose();

    }

}